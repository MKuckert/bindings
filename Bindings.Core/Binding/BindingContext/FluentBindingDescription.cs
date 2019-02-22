// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.Expressions;
using Bindings.Core.Binding.Binders;
using Bindings.Core.Binding.Bindings;
using Bindings.Core.Binding.Combiners;
using Bindings.Core.Binding.ValueConverters;
using Bindings.Core.Converters;
using Bindings.Core.IoC;
using Bindings.Core.Logging;

namespace Bindings.Core.Binding.BindingContext
{
    public class FluentBindingDescription<TTarget, TSource>
        : BaseFluentBindingDescription<TTarget>
        where TTarget : class
    {
        public FluentBindingDescription(IBindingContextOwner bindingContextOwner, TTarget target)
            : base(bindingContextOwner, target)
        {
        }

        public FluentBindingDescription<TTarget, TSource> For(string targetPropertyName)
        {
            BindingDescription.TargetName = targetPropertyName;
            return this;
        }

        public FluentBindingDescription<TTarget, TSource> For(Expression<Func<TTarget, object>> targetPropertyPath)
        {
            var targetPropertyName = TargetPropertyName(targetPropertyPath);
            return For(targetPropertyName);
        }

        public FluentBindingDescription<TTarget, TSource> TwoWay()
        {
            return Mode(BindingMode.TwoWay);
        }

        public FluentBindingDescription<TTarget, TSource> OneWay()
        {
            return Mode(BindingMode.OneWay);
        }

        public FluentBindingDescription<TTarget, TSource> OneWayToSource()
        {
            return Mode(BindingMode.OneWayToSource);
        }

        public FluentBindingDescription<TTarget, TSource> OneTime()
        {
            return Mode(BindingMode.OneTime);
        }

        public FluentBindingDescription<TTarget, TSource> Mode(BindingMode mode)
        {
            BindingDescription.Mode = mode;
            return this;
        }

        public FluentBindingDescription<TTarget, TSource> To(string sourcePropertyPath)
        {
            SetFreeTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public FluentBindingDescription<TTarget, TSource> To(Expression<Func<TSource, object>> sourceProperty)
        {
            var sourcePropertyPath = SourcePropertyPath(sourceProperty);
            SetKnownTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public FluentBindingDescription<TTarget, TSource> ByCombining(string combinerName, params Expression<Func<TSource, object>>[] properties)
            => ByCombining(combinerName, properties.Select(SourcePropertyPath).ToArray());

        public FluentBindingDescription<TTarget, TSource> ByCombining(string combinerName, params string[] properties)
            => To($"{combinerName}({string.Join(", ", properties)})");

        public FluentBindingDescription<TTarget, TSource> ByCombining(IValueCombiner combiner, params Expression<Func<TSource, object>>[] properties)
        {
            SetCombiner(combiner, properties.Select(SourcePropertyPath).ToArray(), useParser: false);
            return this;
        }

        public FluentBindingDescription<TTarget, TSource> ByCombining(IValueCombiner combiner, params string[] properties)
        {
            SetCombiner(combiner, properties, useParser: true);
            return this;
        }

        public FluentBindingDescription<TTarget, TSource> CommandParameter(object parameter)
        {
            return WithConversion(new CommandParameterValueConverter(), parameter);
        }

        public FluentBindingDescription<TTarget, TSource> WithConversion(string converterName,
                                                                            object converterParameter = null)
        {
            var converter = ValueConverterFromName(converterName);
            return WithConversion(converter, converterParameter);
        }

        public FluentBindingDescription<TTarget, TSource> WithConversion(IValueConverter converter,
                                                                            object converterParameter = null)
        {
            SourceStepDescription.Converter = converter;
            SourceStepDescription.ConverterParameter = converterParameter;
            return this;
        }

        public FluentBindingDescription<TTarget, TSource> WithConversion<TValueConverter>(object converterParameter = null)
            where TValueConverter : IValueConverter
        {
            var filler = IoCProvider.Instance.Resolve<IValueConverterRegistryFiller>();
            var converterName = filler.FindName(typeof(TValueConverter));

            return WithConversion(converterName, converterParameter);
        }

        public FluentBindingDescription<TTarget, TSource> WithFallback(object fallback)
        {
            SourceStepDescription.FallbackValue = fallback;
            return this;
        }

        public FluentBindingDescription<TTarget, TSource> SourceDescribed(string bindingDescription)
        {
            var newBindingDescription =
                BindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
            return SourceDescribed(newBindingDescription);
        }

        public FluentBindingDescription<TTarget, TSource> SourceDescribed(BindingDescription description)
        {
            SourceOverwrite(description ?? new BindingDescription());
            return this;
        }

        public FluentBindingDescription<TTarget, TSource> FullyDescribed(string bindingDescription)
        {
            var newBindingDescription =
                BindingSingletonCache.Instance.BindingDescriptionParser.Parse(bindingDescription)
                .ToList();

            if (newBindingDescription.Count > 1)
            {
                Log.Warning("More than one description found - only first will be used in {0}", bindingDescription);
            }

            return FullyDescribed(newBindingDescription.FirstOrDefault());
        }

        public FluentBindingDescription<TTarget, TSource> FullyDescribed(BindingDescription description)
        {
            FullOverwrite(description ?? new BindingDescription());
            return this;
        }

        public FluentBindingDescription<TTarget, TSource> WithClearBindingKey(object clearBindingKey)
        {
            ClearBindingKey = clearBindingKey;
            return this;
        }
    }

    public class FluentBindingDescription<TTarget>
        : BaseFluentBindingDescription<TTarget>
        where TTarget : class
    {
        public FluentBindingDescription(IBindingContextOwner bindingContextOwner, TTarget target = null)
            : base(bindingContextOwner, target)
        {
        }

        public FluentBindingDescription<TTarget> For(string targetPropertyName)
        {
            BindingDescription.TargetName = targetPropertyName;
            return this;
        }

        public FluentBindingDescription<TTarget> For(Expression<Func<TTarget, object>> targetPropertyPath)
        {
            var targetPropertyName = TargetPropertyName(targetPropertyPath);
            return For(targetPropertyName);
        }

        public FluentBindingDescription<TTarget> TwoWay()
        {
            return Mode(BindingMode.TwoWay);
        }

        public FluentBindingDescription<TTarget> OneWay()
        {
            return Mode(BindingMode.OneWay);
        }

        public FluentBindingDescription<TTarget> OneWayToSource()
        {
            return Mode(BindingMode.OneWayToSource);
        }

        public FluentBindingDescription<TTarget> OneTime()
        {
            return Mode(BindingMode.OneTime);
        }

        public FluentBindingDescription<TTarget> Mode(BindingMode mode)
        {
            BindingDescription.Mode = mode;
            return this;
        }

        public FluentBindingDescription<TTarget> To(string sourcePropertyPath)
        {
            SetFreeTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public FluentBindingDescription<TTarget> To<TSource>(Expression<Func<TSource, object>> sourceProperty)
        {
            var sourcePropertyPath = SourcePropertyPath(sourceProperty);
            SetKnownTextPropertyPath(sourcePropertyPath);
            return this;
        }

        public FluentBindingDescription<TTarget> CommandParameter(object parameter)
        {
            return WithConversion(new CommandParameterValueConverter(), parameter);
        }

        public FluentBindingDescription<TTarget> WithConversion(string converterName,
                                                                   object converterParameter = null)
        {
            var converter = ValueConverterFromName(converterName);
            return WithConversion(converter, converterParameter);
        }

        public FluentBindingDescription<TTarget> WithConversion(IValueConverter converter,
                                                                   object converterParameter)
        {
            SourceStepDescription.Converter = converter;
            SourceStepDescription.ConverterParameter = converterParameter;
            return this;
        }

        public FluentBindingDescription<TTarget> WithConversion<TValueConverter>(object converterParameter = null)
            where TValueConverter : IValueConverter
        {
            var filler = IoCProvider.Instance.Resolve<IValueConverterRegistryFiller>();
            var converterName = filler.FindName(typeof(TValueConverter));

            return WithConversion(converterName, converterParameter);
        }

        public FluentBindingDescription<TTarget> WithFallback(object fallback)
        {
            SourceStepDescription.FallbackValue = fallback;
            return this;
        }

        public FluentBindingDescription<TTarget> SourceDescribed(string bindingDescription)
        {
            var newBindingDescription =
                BindingSingletonCache.Instance.BindingDescriptionParser.ParseSingle(bindingDescription);
            return SourceDescribed(newBindingDescription);
        }

        public FluentBindingDescription<TTarget> SourceDescribed(BindingDescription description)
        {
            SourceOverwrite(description ?? new BindingDescription());
            return this;
        }

        public FluentBindingDescription<TTarget> FullyDescribed(string bindingDescription)
        {
            var newBindingDescription =
                BindingSingletonCache.Instance.BindingDescriptionParser.Parse(bindingDescription)
                .ToList();

            if (newBindingDescription.Count > 1)
            {
                Log.Warning("More than one description found - only first will be used in {0}", bindingDescription);
            }

            return FullyDescribed(newBindingDescription.FirstOrDefault());
        }

        public FluentBindingDescription<TTarget> FullyDescribed(BindingDescription description)
        {
            FullOverwrite(description ?? new BindingDescription());
            return this;
        }

        public FluentBindingDescription<TTarget> WithClearBindingKey(object clearBindingKey)
        {
            ClearBindingKey = clearBindingKey;
            return this;
        }
    }
}
