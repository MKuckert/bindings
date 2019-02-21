// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Bindings.Core.Converters;

namespace Bindings.Core.Binding.Binders
{
    public class ValueConverterRegistry
        : NamedInstanceRegistry<IValueConverter>, IValueConverterLookup, IValueConverterRegistry
    {
    }
}