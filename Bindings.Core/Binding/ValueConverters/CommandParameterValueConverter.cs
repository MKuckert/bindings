// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Windows.Input;
using Bindings.Core.Converters;

namespace Bindings.Core.Binding.ValueConverters
{
    public class CommandParameterValueConverter
        : ValueConverter<ICommand, ICommand>
    {
        protected override ICommand Convert(ICommand value, Type targetType, object parameter,
                                            CultureInfo culture)
        {
            return new WrappingCommand(value, parameter);
        }
    }
}