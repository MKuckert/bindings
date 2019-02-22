// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
namespace Bindings.Core.IoC
{
    public class IocOptions : IIocOptions
    {
        public bool TryToDetectSingletonCircularReferences { get; set; } = true;
        public bool TryToDetectDynamicCircularReferences { get; set; } = true;
    }
}
