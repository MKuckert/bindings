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
    public class PropertyInjector : IPropertyInjector
    {
        public virtual void Inject(object target, IPropertyInjectorOptions options = null)
        {
            options = options ?? PropertyInjectorOptions.All;

            if (options.InjectIntoProperties == PropertyInjection.None)
                return;

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var injectableProperties = FindInjectableProperties(target.GetType(), options);

            foreach (var injectableProperty in injectableProperties)
            {
                InjectProperty(target, injectableProperty, options);
            }
        }

        protected virtual void InjectProperty(object toReturn, PropertyInfo injectableProperty, IPropertyInjectorOptions options)
        {
            if (IoCProvider.Instance.TryResolve(injectableProperty.PropertyType, out var propertyValue))
            {
                try
                {
                    injectableProperty.SetValue(toReturn, propertyValue, null);
                }
                catch (TargetInvocationException invocation)
                {
                    throw new BindingIoCResolveException(invocation, "Failed to inject into {0} on {1}", injectableProperty.Name, toReturn.GetType().Name);
                }
            }
            else
            {
                if (options.ThrowIfPropertyInjectionFails)
                    throw new BindingIoCResolveException("IoC property injection failed for {0} on {1}", injectableProperty.Name, toReturn.GetType().Name);
                else
                    BindingLog.Warning("IoC property injection skipped for {0} on {1}", injectableProperty.Name, toReturn.GetType().Name);
            }
        }

        protected virtual IEnumerable<PropertyInfo> FindInjectableProperties(Type type, IPropertyInjectorOptions options)
        {
            var injectableProperties = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(p => p.PropertyType.GetTypeInfo().IsInterface)
                .Where(p => p.IsConventional())
                .Where(p => p.CanWrite);

            switch (options.InjectIntoProperties)
            {
                case PropertyInjection.InjectInterfaceProperties:
                    injectableProperties = injectableProperties
                        .Where(p => p.GetCustomAttributes(typeof(InjectAttribute), false).Any());
                    break;

                case PropertyInjection.AllInterfaceProperties:
                    break;

                case PropertyInjection.None:
                    BindingLog.Error("Internal error - should not call FindInjectableProperties with MvxPropertyInjection.None");
                    injectableProperties = new PropertyInfo[0];
                    break;

                default:
                    throw new BindingException("unknown option for InjectIntoProperties {0}", options.InjectIntoProperties);
            }
            return injectableProperties;
        }
    }
}
