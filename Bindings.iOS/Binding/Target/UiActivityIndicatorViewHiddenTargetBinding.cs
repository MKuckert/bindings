// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Bindings.Core.Binding;
using Bindings.Core.Binding.Bindings.Target;
using UIKit;

namespace Bindings.iOS.Binding.Target
{
    /// <summary>
    /// Custom binding for UIActivityIndicator hidden.
    /// This binding will ensure the indicator animates when shown and stops when hidden
    /// </summary>
    public class UiActivityIndicatorViewHiddenTargetBinding : ConvertingTargetBinding
    {
        public UiActivityIndicatorViewHiddenTargetBinding(UIActivityIndicatorView target)
            : base(target)
        {
            if (target == null)
            {
                BindingLog.Error(
                                    "Error - UIActivityIndicatorView is null in MvxUIActivityIndicatorViewHiddenTargetBinding");
            }
        }

        public override BindingMode DefaultMode => BindingMode.OneWay;

        public override Type TargetType => typeof(bool);

        protected UIActivityIndicatorView View => Target as UIActivityIndicatorView;

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UIActivityIndicatorView)target;
            if (view == null)
            {
                return;
            }

            view.Hidden = (bool)value;

            if (view.Hidden)
            {
                view.StopAnimating();
            }
            else
            {
                view.StartAnimating();
            }
        }
    }
}
