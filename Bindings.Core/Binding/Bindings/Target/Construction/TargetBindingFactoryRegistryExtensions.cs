// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace Bindings.Core.Binding.Bindings.Target.Construction
{
    public static class TargetBindingFactoryRegistryExtensions
    {
        public static void RegisterCustomBindingFactory<TView>(
            this ITargetBindingFactoryRegistry registry,
            string customName,
            Func<TView, ITargetBinding> creator)
            where TView : class
        {
            registry.RegisterFactory(new CustomBindingFactory<TView>(customName, creator));
        }

        public static void RegisterPropertyInfoBindingFactory(this ITargetBindingFactoryRegistry registry,
                                                              Type bindingType, Type targetType, string targetName)
        {
            registry.RegisterFactory(new SimplePropertyInfoTargetBindingFactory(bindingType, targetType, targetName));
        }
    }
}