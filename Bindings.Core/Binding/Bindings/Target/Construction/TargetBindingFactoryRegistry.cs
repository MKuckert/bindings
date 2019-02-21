// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bindings.Core.Binding.Bindings.Target.Construction
{
    public class TargetBindingFactoryRegistry : ITargetBindingFactoryRegistry
    {
        private readonly Dictionary<int, IPluginTargetBindingFactory> _lookups =
            new Dictionary<int, IPluginTargetBindingFactory>();

        public virtual ITargetBinding CreateBinding(object target, string targetName)
        {
            ITargetBinding binding;
            if (TryCreateSpecificFactoryBinding(target, targetName, out binding))
                return binding;

            if (TryCreateReflectionBasedBinding(target, targetName, out binding))
                return binding;

            return null;
        }

        protected virtual bool TryCreateReflectionBasedBinding(object target, string targetName,
                                                               out ITargetBinding binding)
        {
            if (string.IsNullOrEmpty(targetName))
            {
                BindingLog.Error("Empty binding target passed to MvxTargetBindingFactoryRegistry");
                binding = null;
                return false;
            }

            if (target == null)
            {
                // null passed in so return false - fixes #584
                binding = null;
                return false;
            }

            var targetPropertyInfo = target.GetType().GetProperty(targetName);
            if (targetPropertyInfo != null
                && targetPropertyInfo.CanWrite)
            {
                binding = new WithEventPropertyInfoTargetBinding(target, targetPropertyInfo);
                return true;
            }

            var targetEventInfo = target.GetType().GetEvent(targetName);
            if (targetEventInfo != null)
            {
                // we only handle EventHandler's here
                // other event types will need to be handled by custom bindings
                if (targetEventInfo.EventHandlerType == typeof(EventHandler))
                {
                    binding = new EventHandlerEventInfoTargetBinding(target, targetEventInfo);
                    return true;
                }
            }

            binding = null;
            return false;
        }

        protected virtual bool TryCreateSpecificFactoryBinding(object target, string targetName,
                                                               out ITargetBinding binding)
        {
            if (target == null)
            {
                // null passed in so return false - fixes #584
                binding = null;
                return false;
            }

            var factory = FindSpecificFactory(target.GetType(), targetName);
            if (factory != null)
            {
                binding = factory.CreateBinding(target, targetName);
                return true;
            }

            binding = null;
            return false;
        }

        public void RegisterFactory(IPluginTargetBindingFactory factory)
        {
            foreach (var supported in factory.SupportedTypes)
            {
                var key = GenerateKey(supported.Type, supported.Name);
                _lookups[key] = factory;
            }
        }

        private static int GenerateKey(Type type, string name)
        {
			return (type.GetHashCode () * 9) ^ name.GetHashCode ();
        }

        private IPluginTargetBindingFactory FindSpecificFactory(Type type, string name)
        {
            IPluginTargetBindingFactory factory;
            var key = GenerateKey(type, name);
            if (_lookups.TryGetValue(key, out factory))
            {
                return factory;
            }
            var baseType = type.GetTypeInfo().BaseType;
            if (baseType != null)
                factory = FindSpecificFactory(baseType, name);
            if (factory != null) return factory;
            var implementedInterfaces = type.GetTypeInfo().ImplementedInterfaces;
            foreach (var implementedInterface in implementedInterfaces)
            {
                factory = FindSpecificFactory(implementedInterface, name);
                if (factory != null) return factory;
            }
            return null;
        }
    }
}
