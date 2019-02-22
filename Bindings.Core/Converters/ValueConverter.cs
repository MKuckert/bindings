// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Bindings.Core.Logging;

namespace Bindings.Core.Converters
{
    public abstract class ValueConverter
        : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BindingConstant.UnsetValue;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BindingConstant.UnsetValue;
        }
    }

    public abstract class ValueConverter<TFrom, TTo>
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Convert((TFrom)value, targetType, parameter, culture);
            }
            catch (Exception e)
            {
                Log.Error($"Failed to Convert from {typeof(TFrom)} to {typeof(TTo)} with Exception: {e}");
                return BindingConstant.UnsetValue;
            }
        }

        protected virtual TTo Convert(TFrom value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return ConvertBack((TTo)value, targetType, parameter, culture);
            }
            catch (Exception e)
            {
                Log.Error($"Failed to ConvertBack from {typeof(TTo)} to {typeof(TFrom)} with Exception: {e}");
                return BindingConstant.UnsetValue;
            }
        }

        protected virtual TFrom ConvertBack(TTo value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class ValueConverter<TFrom>
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Convert((TFrom)value, targetType, parameter, culture);
            }
            catch (Exception e)
            {
                Log.Error($"Failed to Convert from {typeof(TFrom)} with Exception: {e}");
                return BindingConstant.UnsetValue;
            }
        }

        protected virtual object Convert(TFrom value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return TypedConvertBack(value, targetType, parameter, culture);
            }
            catch (Exception e)
            {
                Log.Error($"Failed to ConvertBack to {typeof(TFrom)} with Exception: {e}");
                return BindingConstant.UnsetValue;
            }
        }

        protected virtual TFrom TypedConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
