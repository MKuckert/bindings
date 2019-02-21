// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bindings.Core.Binding;
using Bindings.Core.Exeptions;

namespace Bindings.Core.IoC
{
    public class IoCContainer
        : IIoCProvider
    {
        private readonly object _lock = new object();
        private readonly Dictionary<Type, IResolver> _resolvers = new Dictionary<Type, IResolver>();
        // ReSharper disable once CollectionNeverQueried.Local
        private readonly Dictionary<Type, bool> _circularTypeDetection = new Dictionary<Type, bool>();
        private readonly IPropertyInjector _propertyInjector;
        private readonly IIoCProvider _parentProvider;

        private IIocOptions Options { get; }

        public IoCContainer(IIocOptions options, IIoCProvider parentProvider = null)
        {
            Options = options ?? new IocOptions();
            if (Options.PropertyInjectorType != null)
            {
                _propertyInjector = Activator.CreateInstance(Options.PropertyInjectorType) as IPropertyInjector;
            }
            if (_propertyInjector != null)
            {
                InternalSetResolver(typeof(IPropertyInjector), new SingletonResolver(_propertyInjector));
            }
            if (parentProvider != null)
            {
                _parentProvider = parentProvider;
            }
        }

        private interface IResolver
        {
            object Resolve();

            ResolverType ResolveType { get; }

            void SetGenericTypeParameters(Type[] genericTypeParameters);
        }

        private class ConstructingResolver : IResolver
        {
            private readonly Type _type;
            private readonly IIoCProvider _parent;

            public ConstructingResolver(Type type, IIoCProvider parent)
            {
                _type = type;
                _parent = parent;
            }

            public object Resolve()
            {
                return _parent.IoCConstruct(_type);
            }

            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                throw new InvalidOperationException("This Resolver does not set generic type parameters");
            }

            public ResolverType ResolveType => ResolverType.DynamicPerResolve;
        }

        private class SingletonResolver : IResolver
        {
            private readonly object _theObject;

            public SingletonResolver(object theObject)
            {
                _theObject = theObject;
            }

            public object Resolve()
            {
                return _theObject;
            }

            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                throw new InvalidOperationException("This Resolver does not set generic type parameters");
            }

            public ResolverType ResolveType => ResolverType.Singleton;
        }

        private class ConstructingOpenGenericResolver : IResolver
        {
            private readonly Type _genericTypeDefinition;
            private readonly IIoCProvider _parent;

            private Type[] _genericTypeParameters;

            public ConstructingOpenGenericResolver(Type genericTypeDefinition, IIoCProvider parent)
            {
                _genericTypeDefinition = genericTypeDefinition;
                _parent = parent;
            }

            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                _genericTypeParameters = genericTypeParameters;
            }

            public object Resolve()
            {
                return _parent.IoCConstruct(_genericTypeDefinition.MakeGenericType(_genericTypeParameters));
            }

            public ResolverType ResolveType => ResolverType.DynamicPerResolve;
        }

        public bool CanResolve<T>()
            where T : class
        {
            return CanResolve(typeof(T));
        }

        public bool CanResolve(Type t)
        {
            lock (_lock)
            {
                if (_resolvers.ContainsKey(t))
                {
                    return true;
                }
                if (_parentProvider != null && _parentProvider.CanResolve(t))
                {
                    return true;
                }
                return false;
            }
        }

        public bool TryResolve(Type type, out object resolved)
        {
            lock (_lock)
            {
                return InternalTryResolve(type, out resolved);
            }
        }

        public T Resolve<T>()
            where T : class
        {
            var t = typeof(T);
            object ret;
            lock (_lock)
            {
                if (!InternalTryResolve(t, out var resolved))
                {
                    throw new BindingIoCResolveException("Failed to resolve type {0}", t.FullName);
                }

                ret = resolved;
            }

            return (T)ret;
        }

        public void RegisterType<TInterface, TToConstruct>()
            where TInterface : class
            where TToConstruct : class, TInterface
        {
            var interfaceType = typeof(TInterface);
            var constructType = typeof(TToConstruct);
            IResolver resolver;
            if (interfaceType.GetTypeInfo().IsGenericTypeDefinition)
            {
                resolver = new ConstructingOpenGenericResolver(constructType, this);
            }
            else
            {
                resolver = new ConstructingResolver(constructType, this);
            }

            InternalSetResolver(interfaceType, resolver);
        }

        public void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class
        {
            InternalSetResolver(typeof(TInterface), new SingletonResolver(theObject));
        }

        public object IoCConstruct(Type type)
        {
            var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            var firstConstructor = constructors.FirstOrDefault();

            if (firstConstructor == null)
                throw new BindingIoCResolveException("Failed to find constructor for type {0}", type.FullName);

            var parameters = GetIoCParameterValues(type, firstConstructor, default(IDictionary<string, object>));
            return IoCConstruct(type, firstConstructor, parameters.ToArray());
        }

        protected virtual object IoCConstruct(Type type, ConstructorInfo constructor, object[] arguments)
        {
            object toReturn;
            try
            {
                toReturn = constructor.Invoke(arguments);
            }
            catch (TargetInvocationException invocation)
            {
                throw new BindingIoCResolveException(invocation, "Failed to construct {0}", type.Name);
            }

            try
            {
                InjectProperties(toReturn);
            }
            catch (Exception)
            {
                if (!Options.CheckDisposeIfPropertyInjectionFails)
                    throw;

                if (toReturn is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                throw;
            }
            return toReturn;
        }

        private enum ResolverType
        {
            DynamicPerResolve,
            Singleton,
            Unknown
        }

        private static readonly ResolverType? ResolverTypeNoneSpecified = null;

        private bool Supports(IResolver resolver, ResolverType? requiredResolverType)
        {
            if (!requiredResolverType.HasValue)
                return true;
            return resolver.ResolveType == requiredResolverType.Value;
        }

        private bool InternalTryResolve(Type type, out object resolved)
        {
            return InternalTryResolve(type, ResolverTypeNoneSpecified, out resolved);
        }

        private bool InternalTryResolve(Type type, ResolverType? requiredResolverType, out object resolved)
        {
            if (!TryGetResolver(type, out var resolver))
            {
                if (_parentProvider != null && _parentProvider.TryResolve(type, out resolved))
                {
                    return true;
                }

                resolved = type.CreateDefault();
                return false;
            }

            if (!Supports(resolver, requiredResolverType))
            {
                resolved = type.CreateDefault();
                return false;
            }

            return InternalTryResolve(type, resolver, out resolved);
        }

        private bool TryGetResolver(Type type, out IResolver resolver)
        {
            if (_resolvers.TryGetValue(type, out resolver))
            {
                return true;
            }

            if (!type.GetTypeInfo().IsGenericType)
            {
                return false;
            }

            return _resolvers.TryGetValue(type.GetTypeInfo().GetGenericTypeDefinition(), out resolver);
        }

        private bool ShouldDetectCircularReferencesFor(IResolver resolver)
        {
            switch (resolver.ResolveType)
            {
                case ResolverType.DynamicPerResolve:
                    return Options.TryToDetectDynamicCircularReferences;

                case ResolverType.Singleton:
                    return Options.TryToDetectSingletonCircularReferences;

                case ResolverType.Unknown:
                    throw new BindingException("A resolver must have a known type - error in {0}", resolver.GetType().Name);
                default:
                    throw new ArgumentOutOfRangeException(nameof(resolver), "unknown resolveType of " + resolver.ResolveType);
            }
        }

        private bool InternalTryResolve(Type type, IResolver resolver, out object resolved)
        {
            var detectingCircular = ShouldDetectCircularReferencesFor(resolver);
            if (detectingCircular)
            {
                try
                {
                    _circularTypeDetection.Add(type, true);
                }
                catch (ArgumentException)
                {
                    // the item already exists in the lookup table
                    // - this is "game over" for the IoC lookup
                    // - see https://github.com/MvvmCross/MvvmCross/issues/553
                    BindingLog.Error("IoC circular reference detected - cannot currently resolve {0}", type.Name);
                    resolved = type.CreateDefault();
                    return false;
                }
            }

            try
            {
                if (resolver is ConstructingOpenGenericResolver)
                {
                    resolver.SetGenericTypeParameters(type.GetTypeInfo().GenericTypeArguments);
                }

                var raw = resolver.Resolve();
                if (!type.IsInstanceOfType(raw))
                {
                    throw new BindingException("Resolver returned object type {0} which does not support interface {1}",
                                           raw.GetType().FullName, type.FullName);
                }

                resolved = raw;
                return true;
            }
            finally
            {
                if (detectingCircular)
                {
                    _circularTypeDetection.Remove(type);
                }
            }
        }

        private void InternalSetResolver(Type interfaceType, IResolver resolver)
        {
            lock (_lock)
            {
                _resolvers[interfaceType] = resolver;
            }
        }

        protected virtual void InjectProperties(object toReturn)
        {
            _propertyInjector?.Inject(toReturn, Options.PropertyInjectorOptions);
        }

        protected virtual List<object> GetIoCParameterValues(Type type, ConstructorInfo firstConstructor, IDictionary<string, object> arguments)
        {
            var parameters = new List<object>();
            foreach (var parameterInfo in firstConstructor.GetParameters())
            {
                object parameterValue;
                if (arguments != null && arguments.ContainsKey(parameterInfo.Name))
                {
                    parameterValue = arguments[parameterInfo.Name];
                }
                else if (!TryResolve(parameterInfo.ParameterType, out parameterValue))
                {
                    if (parameterInfo.IsOptional)
                    {
                        parameterValue = Type.Missing;
                    }
                    else
                    {
                        throw new BindingIoCResolveException(
                            "Failed to resolve parameter for parameter {0} of type {1} when creating {2}. You may pass it as an argument",
                            parameterInfo.Name,
                            parameterInfo.ParameterType.Name,
                            type.FullName);
                    }
                }

                parameters.Add(parameterValue);
            }
            return parameters;
        }
    }
}
