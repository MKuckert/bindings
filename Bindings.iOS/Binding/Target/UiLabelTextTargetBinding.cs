// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Bindings.Core.Binding;
using Bindings.Core.Binding.Bindings.Target;
using UIKit;

namespace Bindings.iOS.Binding.Target
{
    public class UiLabelTextTargetBinding
        : ConvertingTargetBinding
    {
        protected UILabel View => Target as UILabel;

        public UiLabelTextTargetBinding(UILabel target)
            : base(target)
        {
            if (target == null)
            {
                BindingLog.Error(
                                      "Error - UILabel is null in MvxUILabelTextTargetBinding");
            }
        }

        public override BindingMode DefaultMode => BindingMode.OneWay;

        public override Type TargetType => typeof(string);

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UILabel)target;
            if (view == null)
                return;

            view.Text = (string)value;
        }
    }
}
