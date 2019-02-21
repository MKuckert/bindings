// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Bindings.Core.IoC
{
    public class PropertyInjectorOptions : IPropertyInjectorOptions
    {
        public PropertyInjectorOptions()
        {
            InjectIntoProperties = PropertyInjection.None;
            ThrowIfPropertyInjectionFails = false;
        }

        public PropertyInjection InjectIntoProperties { get; set; }
        public bool ThrowIfPropertyInjectionFails { get; set; }

        private static IPropertyInjectorOptions _injectProperties;

        public static IPropertyInjectorOptions Inject
        {
            get
            {
                _injectProperties = _injectProperties ?? new PropertyInjectorOptions()
                {
                    InjectIntoProperties = PropertyInjection.InjectInterfaceProperties,
                    ThrowIfPropertyInjectionFails = false
                };
                return _injectProperties;
            }
        }

        private static IPropertyInjectorOptions _allProperties;

        public static IPropertyInjectorOptions All
        {
            get
            {
                _allProperties = _allProperties ?? new PropertyInjectorOptions()
                {
                    InjectIntoProperties = PropertyInjection.AllInterfaceProperties,
                    ThrowIfPropertyInjectionFails = false
                };
                return _allProperties;
            }
        }
    }
}
