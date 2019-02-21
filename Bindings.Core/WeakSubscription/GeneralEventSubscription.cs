// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;

namespace Bindings.Core.WeakSubscription
{
    public class GeneralEventSubscription
        : WeakEventSubscription<object, EventArgs>
    {
        public GeneralEventSubscription(object source,
                                           EventInfo eventInfo,
                                           EventHandler<EventArgs> eventHandler)
            : base(source, eventInfo, eventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler(OnSourceEvent);
        }
    }
}
