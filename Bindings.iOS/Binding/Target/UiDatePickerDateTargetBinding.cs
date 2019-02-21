// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Foundation;
using UIKit;

namespace Bindings.iOS.Binding.Target
{
    public class UiDatePickerDateTargetBinding : BaseUiDatePickerTargetBinding
    {
        public UiDatePickerDateTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override object GetValueFrom(UIDatePicker view)
        {
            // Convert from universal NSDate back to a local DateTime based on system timezone.
            var valueUtc = view.Date.ToDateTimeUtc();
            var valueLocal = ToLocalTime(valueUtc);
            return valueLocal;
        }

        public static DateTime DefaultDate { get; set; } = DateTime.Now;

        protected override object MakeSafeValue(object value)
        {
            // Convert from local DateTime (or default value) to universal NSDate based on system timezone.
            var valueLocal = (DateTime)(value ?? DefaultDate);
            var valueUtc = ToUtcTime(valueLocal);
            var valueNsDate = valueUtc.ToNsDate();

            if (View.MaximumDate != null && View.MaximumDate.Compare(valueNsDate) == NSComparisonResult.Ascending)
                valueNsDate = View.MaximumDate;
            else if (View.MinimumDate != null && View.MinimumDate.Compare(valueNsDate) == NSComparisonResult.Descending)
                valueNsDate = View.MinimumDate;

            return valueNsDate;
        }
    }
}
