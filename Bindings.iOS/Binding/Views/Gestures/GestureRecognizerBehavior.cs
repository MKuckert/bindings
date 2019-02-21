// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Windows.Input;
using UIKit;

namespace Bindings.iOS.Binding.Views.Gestures
{
    public abstract class GestureRecognizerBehavior
    {
        public ICommand Command { get; set; }

        protected void FireCommand(object argument = null)
        {
            var command = Command;
            command?.Execute(null);
        }

        protected void AddGestureRecognizer(UIView target, UIGestureRecognizer tap)
        {
            if (!target.UserInteractionEnabled)
                target.UserInteractionEnabled = true;

            target.AddGestureRecognizer(tap);
        }
    }

    public abstract class GestureRecognizerBehavior<T>
        : GestureRecognizerBehavior
    {
        protected virtual void HandleGesture(T gesture)
        {
        }
    }
}
