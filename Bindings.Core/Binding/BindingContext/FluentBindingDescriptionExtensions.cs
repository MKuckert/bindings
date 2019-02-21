// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Bindings.Core.Converters;

namespace Bindings.Core.Binding.BindingContext
{
    public static class FluentBindingDescriptionExtensions
    {
        public static FluentBindingDescription<TTarget, TSource> WithDictionaryConversion<TTarget, TSource, TFrom, TTo>(
            this FluentBindingDescription<TTarget, TSource> bindingDescription,
            IDictionary<TFrom, TTo> converterParameter)
            where TTarget : class
            => bindingDescription.WithConversion(new DictionaryValueConverter<TFrom, TTo>(), new Tuple<IDictionary<TFrom, TTo>, TTo, bool>(converterParameter, default(TTo), false))
            .OneWay();

        public static FluentBindingDescription<TTarget, TSource> WithDictionaryConversion<TTarget, TSource, TFrom, TTo>(
            this FluentBindingDescription<TTarget, TSource> bindingDescription,
            IDictionary<TFrom, TTo> converterParameter,
            TTo fallback)
            where TTarget : class
            => bindingDescription.WithConversion(new DictionaryValueConverter<TFrom, TTo>(), new Tuple<IDictionary<TFrom, TTo>, TTo, bool>(converterParameter, fallback, true))
            .OneWay();
    }
}
