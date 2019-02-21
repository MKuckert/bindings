// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Bindings.Core.Binding.BindingContext
{
    public static partial class BindingContextOwnerExtensions
    {
        public static FluentBindingDescriptionSet<TTarget, TSource> CreateBindingSet<TTarget, TSource>(
            this TTarget target)
            where TTarget : class, IBindingContextOwner
        {
            return new FluentBindingDescriptionSet<TTarget, TSource>(target);
        }

        public static FluentBindingDescriptionSet<TTarget, TSource> CreateBindingSet<TTarget, TSource>(this TTarget target, TSource justUsedForTypeInference)
            where TTarget : class, IBindingContextOwner
        {
            return new FluentBindingDescriptionSet<TTarget, TSource>(target);
        }

        public static FluentBindingDescription<TTarget> CreateBinding<TTarget>(this TTarget target)
            where TTarget : class, IBindingContextOwner
        {
            return new FluentBindingDescription<TTarget>(target, target);
        }

        public static FluentBindingDescription<TTarget> CreateBinding<TTarget>(
            this IBindingContextOwner contextOwner, TTarget target)
            where TTarget : class
        {
            return new FluentBindingDescription<TTarget>(contextOwner, target);
        }
    }
}