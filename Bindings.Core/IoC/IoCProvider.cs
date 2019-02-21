// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Bindings.Core.Base;

namespace Bindings.Core.IoC
{
    /// <summary>
    /// Singleton IoC Provider.
    /// 
    /// Delegates to the <see cref="IoCContainer"/> implementation
    /// </summary>
    public sealed class IoCProvider
        : Singleton<IIoCProvider>, IIoCProvider
    {
        public static void Initialize(IIocOptions options = null)
        {
            if (Instance != null)
            {
                return;
            }

            // create a new ioc container - it will register itself as the singleton
            // ReSharper disable ObjectCreationAsStatement
            new IoCProvider(options);
            // ReSharper restore ObjectCreationAsStatement
        }

        private readonly IoCContainer _provider;

        private IoCProvider(IIocOptions options)
        {
            _provider = new IoCContainer(options);
        }

        public bool CanResolve<T>()
            where T : class
        {
            return _provider.CanResolve<T>();
        }

        public bool CanResolve(Type t)
        {
            return _provider.CanResolve(t);
        }

        public bool TryResolve(Type type, out object resolved)
        {
            return _provider.TryResolve(type, out resolved);
        }

        public T Resolve<T>()
            where T : class
        {
            return _provider.Resolve<T>();
        }

        public void RegisterType<TInterface, TToConstruct>()
            where TInterface : class
            where TToConstruct : class, TInterface
        {
            _provider.RegisterType<TInterface, TToConstruct>();
        }

        public void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class
        {
            _provider.RegisterSingleton(theObject);
        }

        public object IoCConstruct(Type type)
        {
            return _provider.IoCConstruct(type);
        }
    }
}
