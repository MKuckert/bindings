// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Bindings.Core.Base;
using Bindings.Core.Converters;

namespace Bindings.Core.Binding.Binders
{
    public interface INamedInstanceRegistryFiller<out T>
    {
        string FindName(Type type);

        void FillFrom(INamedInstanceRegistry<T> registry, Type type);

        void FillFrom(INamedInstanceRegistry<T> registry, Assembly assembly);
    }

    public interface IValueConverterRegistryFiller : INamedInstanceRegistryFiller<IValueConverter>
    {
    }
}