// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Bindings.Core.Base;

namespace Bindings.Core.WeakSubscription
{
    public class ValueEventSubscription<T>
        : WeakEventSubscription<object, ValueEventArgs<T>>
    {
        public ValueEventSubscription(object source,
                                         EventInfo eventInfo,
                                         EventHandler<ValueEventArgs<T>> eventHandler)
            : base(source, eventInfo, eventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler<ValueEventArgs<T>>(OnSourceEvent);
        }
    }
}
