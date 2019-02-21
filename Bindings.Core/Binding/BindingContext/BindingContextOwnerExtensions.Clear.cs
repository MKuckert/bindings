// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Bindings.Core.Binding.BindingContext
{
    public static partial class BindingContextOwnerExtensions
    {
        public static void ClearBindings(this IBindingContextOwner owner, object target)
        {
            owner.BindingContext.ClearBindings(target);
        }

        public static void ClearAllBindings(this IBindingContextOwner owner)
        {
            owner.BindingContext.ClearAllBindings();
        }
    }
}