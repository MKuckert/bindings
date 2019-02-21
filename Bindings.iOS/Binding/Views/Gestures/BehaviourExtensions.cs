// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using UIKit;

namespace Bindings.iOS.Binding.Views.Gestures
{
    public static class BehaviourExtensions
    {
        public static TapGestureRecognizerBehaviour Tap(this UIView view, uint numberOfTapsRequired = 1,
                                                           uint numberOfTouchesRequired = 1,
                                                           bool cancelsTouchesInView = true)
        {
            var toReturn = new TapGestureRecognizerBehaviour(view, numberOfTapsRequired, numberOfTouchesRequired, cancelsTouchesInView);
            return toReturn;
        }

        public static SwipeGestureRecognizerBehaviour Swipe(this UIView view, UISwipeGestureRecognizerDirection direction,
                                                               uint numberOfTouchesRequired = 1)
        {
            var toReturn = new SwipeGestureRecognizerBehaviour(view, direction, numberOfTouchesRequired);
            return toReturn;
        }
    }
}
