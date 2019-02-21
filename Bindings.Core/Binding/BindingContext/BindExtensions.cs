// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bindings.Core.Binding.Bindings;
using Bindings.Core.Converters;

namespace Bindings.Core.Binding.BindingContext
{
    // ReSharper disable once UnusedMember.Global
    public static class BindExtensions
    {
        public static InlineBindingTarget CreateInlineBindingTarget(
            this IBindingContextOwner bindingContextOwner)
        {
            return new InlineBindingTarget(bindingContextOwner);
        }

        public static T Bind<T>(this T element, InlineBindingTarget target,
                                            string descriptionText)
        {
            target.BindingContextOwner.AddBindings(element, descriptionText);
            return element;
        }

        public static T Bind<T, TViewModel>(this T element,
                                            InlineBindingTarget target,
                                            Expression<Func<TViewModel, object>> sourcePropertyPath,
                                            string converterName = null,
                                            object converterParameter = null,
                                            object fallbackValue = null,
                                            BindingMode mode = BindingMode.Default)
        {
            return element.Bind(target, null, sourcePropertyPath, converterName, converterParameter, fallbackValue, mode);
        }

        public static T Bind<T, TViewModel>(this T element,
                                            InlineBindingTarget target,
                                            Expression<Func<TViewModel, object>> sourcePropertyPath,
                                            IValueConverter converter,
                                            object converterParameter = null,
                                            object fallbackValue = null,
                                            BindingMode mode = BindingMode.Default)
        {
            return element.Bind(target, null, sourcePropertyPath, converter, converterParameter, fallbackValue, mode);
        }

        public static T Bind<T, TViewModel>(this T element,
                                            InlineBindingTarget target,
                                            Expression<Func<T, object>> targetPropertyPath,
                                            Expression<Func<TViewModel, object>> sourcePropertyPath,
                                            string converterName = null,
                                            object converterParameter = null,
                                            object fallbackValue = null,
                                            BindingMode mode = BindingMode.Default)
        {
            var converter = BindingSingletonCache.Instance.ValueConverterLookup.Find(converterName);
            return element.Bind(target, targetPropertyPath, sourcePropertyPath, converter, converterParameter,
                                fallbackValue, mode);
        }

        public static T Bind<T, TViewModel>(this T element,
                                            InlineBindingTarget target,
                                            Expression<Func<T, object>> targetPropertyPath,
                                            Expression<Func<TViewModel, object>> sourcePropertyPath,
                                            IValueConverter converter,
                                            object converterParameter = null,
                                            object fallbackValue = null,
                                            BindingMode mode = BindingMode.Default)
        {
            var parser = BindingSingletonCache.Instance.PropertyExpressionParser;
            var sourcePath = parser.Parse(sourcePropertyPath).Print();
            var targetPath = targetPropertyPath == null ? null : parser.Parse(targetPropertyPath).Print();
            return element.Bind(target, targetPath, sourcePath, converter, converterParameter, fallbackValue, mode);
        }

        public static T Bind<T>(this T element,
                                            InlineBindingTarget target,
                                            string targetPath,
                                            string sourcePath,
                                            IValueConverter converter = null,
                                            object converterParameter = null,
                                            object fallbackValue = null,
                                            BindingMode mode = BindingMode.Default)
        {
            if (string.IsNullOrEmpty(targetPath))
                targetPath = BindingSingletonCache.Instance.DefaultBindingNameLookup.DefaultFor(typeof(T));

            var bindingDescription = new BindingDescription(
                targetPath,
                sourcePath,
                converter,
                converterParameter,
                fallbackValue,
                mode);

            target.BindingContextOwner.AddBinding(element, bindingDescription);

            return element;
        }

        public static T Bind<T>(this T element, IBindingContextOwner bindingContextOwner, string descriptionText)
        {
            bindingContextOwner.AddBindings(element, descriptionText);
            return element;
        }

        public static T Bind<T>(this T element, IBindingContextOwner bindingContextOwner,
                                IEnumerable<BindingDescription> descriptions)
        {
            bindingContextOwner.AddBindings(element, descriptions);
            return element;
        }
    }
}