// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using UIKit;

namespace Bindings.iOS.Binding.Target
{
    public class UiDatePickerCountDownDurationTargetBinding : BaseUiDatePickerTargetBinding
    {
        public UiDatePickerCountDownDurationTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override object GetValueFrom(UIDatePicker view)
        {
            return view.CountDownDuration;
        }

        public override Type TargetType => typeof(double);
    }
}
