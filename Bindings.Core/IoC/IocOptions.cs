// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace Bindings.Core.IoC
{
    public class IocOptions : IIocOptions
    {
        public IocOptions()
        {
            TryToDetectSingletonCircularReferences = true;
            TryToDetectDynamicCircularReferences = true;
            CheckDisposeIfPropertyInjectionFails = true;
            PropertyInjectorType = typeof(PropertyInjector);
            PropertyInjectorOptions = new PropertyInjectorOptions();
        }

        public bool TryToDetectSingletonCircularReferences { get; set; }
        public bool TryToDetectDynamicCircularReferences { get; set; }
        public bool CheckDisposeIfPropertyInjectionFails { get; set; }
        public Type PropertyInjectorType { get; set; }
        public IPropertyInjectorOptions PropertyInjectorOptions { get; set; }
    }
}
