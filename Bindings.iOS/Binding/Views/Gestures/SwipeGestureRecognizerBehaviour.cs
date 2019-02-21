// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using UIKit;

namespace Bindings.iOS.Binding.Views.Gestures
{
    public class SwipeGestureRecognizerBehaviour
        : GestureRecognizerBehavior<UISwipeGestureRecognizer>
    {
        protected override void HandleGesture(UISwipeGestureRecognizer gesture)
        {
            FireCommand();
        }

        public SwipeGestureRecognizerBehaviour(UIView target, UISwipeGestureRecognizerDirection direction,
                                                uint numberOfTouchesRequired = 1)
        {
            var swipe = new UISwipeGestureRecognizer(HandleGesture)
            {
                Direction = direction,
                NumberOfTouchesRequired = numberOfTouchesRequired
            };

            AddGestureRecognizer(target, swipe);
        }
    }
}
