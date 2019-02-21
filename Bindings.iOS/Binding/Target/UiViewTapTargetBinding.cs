// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using Bindings.Core.Binding;
using Bindings.Core.Binding.Bindings.Target;
using Bindings.iOS.Binding.Views.Gestures;
using UIKit;

namespace Bindings.iOS.Binding.Target
{
    public class UiViewTapTargetBinding : ConvertingTargetBinding
    {
        private readonly TapGestureRecognizerBehaviour _behaviour;

        public UiViewTapTargetBinding(UIView target, uint numberOfTapsRequired = 1, uint numberOfTouchesRequired = 1, bool cancelsTouchesInView = true)
            : base(target)
        {
            _behaviour = new TapGestureRecognizerBehaviour(target, numberOfTapsRequired, numberOfTouchesRequired, cancelsTouchesInView);
        }

        public override BindingMode DefaultMode => BindingMode.OneWay;

        public override Type TargetType => typeof(ICommand);

        protected override void SetValueImpl(object target, object value)
        {
            _behaviour.Command = (ICommand)value;
        }
    }
}
