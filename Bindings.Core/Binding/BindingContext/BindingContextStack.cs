// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Bindings.Core.Binding.BindingContext
{
    public class BindingContextStack<TContext>
        : Stack<TContext>, IBindingContextStack<TContext>
    {
        public TContext Current
        {
            get
            {
                if (Count == 0)
                    return default(TContext);
                return Peek();
            }
        }
    }
}