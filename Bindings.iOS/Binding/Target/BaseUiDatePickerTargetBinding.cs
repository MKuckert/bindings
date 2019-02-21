// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Bindings.Core.Binding;
using Bindings.Core.Binding.Bindings.Target;
using Bindings.Core.WeakSubscription;
using Foundation;
using UIKit;

namespace Bindings.iOS.Binding.Target
{
    public abstract class BaseUiDatePickerTargetBinding : PropertyInfoTargetBinding<UIDatePicker>
    {
        private readonly NSTimeZone _systemTimeZone;
        private WeakEventSubscription<UIDatePicker> _subscription;

        protected BaseUiDatePickerTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            _systemTimeZone = NSTimeZone.SystemTimeZone;
        }

        private void DatePickerOnValueChanged(object sender, EventArgs args)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged(GetValueFrom(view));
        }

        protected abstract object GetValueFrom(UIDatePicker view);

        public override BindingMode DefaultMode => BindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var datePicker = View;
            if (datePicker == null)
            {
                BindingLog.Error( "Error - UIDatePicker is null in MvxBaseUIDatePickerTargetBinding");
            }
            // Only listen for value changes if we are binding against one of the value-derived properties.
            else if (TargetPropertyInfo.Name == nameof(UIDatePicker.Date) || TargetPropertyInfo.Name == nameof(UIDatePicker.CountDownDuration))
            {
                _subscription = datePicker.WeakSubscribe(nameof(datePicker.ValueChanged), DatePickerOnValueChanged);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (!isDisposing) return;

            _subscription?.Dispose();
        }

        protected DateTime ToLocalTime(DateTime utc)
        {
            if (utc.Kind == DateTimeKind.Local)
                return utc;

            var local = utc.AddSeconds(_systemTimeZone.SecondsFromGMT(utc.ToNsDate())).WithKind(DateTimeKind.Local);

            return local;
        }

        protected DateTime ToUtcTime(DateTime local)
        {
            if (local.Kind == DateTimeKind.Utc)
                return local;

            var utc = local.AddSeconds(-_systemTimeZone.SecondsFromGMT(local.ToNsDate())).WithKind(DateTimeKind.Utc);

            return utc;
        }
    }
}
