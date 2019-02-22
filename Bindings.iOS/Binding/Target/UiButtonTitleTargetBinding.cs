// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Bindings.Core;
using Bindings.Core.Binding;
using Bindings.Core.Binding.Bindings.Target;
using Bindings.Core.Logging;
using UIKit;

namespace Bindings.iOS.Binding.Target
{
    public class UiButtonTitleTargetBinding : ConvertingTargetBinding
    {
        private readonly UIControlState _state;

        protected UIButton Button => Target as UIButton;

        public UiButtonTitleTargetBinding(UIButton button, UIControlState state = UIControlState.Normal)
            : base(button)
        {
            _state = state;
            if (button == null)
            {
                Log.Error( $"{nameof(UIButton)} is null in {GetType().Name}");
            }
        }

        public override BindingMode DefaultMode => BindingMode.OneWay;

        public override Type TargetType => typeof(string);

        protected override void SetValueImpl(object target, object value)
        {
            ((UIButton)target).SetTitle(value as string, _state);
        }
    }
}
