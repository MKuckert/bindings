// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Windows.Input;

namespace Bindings.Core.WeakSubscription
{
    public class CanExecuteChangedEventSubscription
        : WeakEventSubscription<ICommand, EventArgs>
    {
        private static readonly EventInfo CanExecuteChangedEventInfo = typeof(ICommand).GetEvent(nameof(ICommand.CanExecuteChanged));

        public CanExecuteChangedEventSubscription(ICommand source,
                                                    EventHandler<EventArgs> eventHandler)
            : base(source, CanExecuteChangedEventInfo, eventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler(OnSourceEvent);
        }
    }
}
