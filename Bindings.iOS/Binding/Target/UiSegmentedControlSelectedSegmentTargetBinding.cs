// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Bindings.Core.Binding;
using Bindings.Core.Binding.Bindings.Target;
using Bindings.Core.WeakSubscription;
using UIKit;

namespace Bindings.iOS.Binding.Target
{
    public class UiSegmentedControlSelectedSegmentTargetBinding : PropertyInfoTargetBinding<UISegmentedControl>
    {
        private IDisposable _subscription;

        public UiSegmentedControlSelectedSegmentTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        public override BindingMode DefaultMode => BindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var segmentedControl = View;
            if (segmentedControl == null)
            {
                BindingLog.Error( "Error - UISegmentedControl is null in MvxUISegmentedControlSelectedSegmentTargetBinding");
                return;
            }

            _subscription = segmentedControl.WeakSubscribe(nameof(segmentedControl.ValueChanged), HandleValueChanged);
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as UISegmentedControl;
            if (view == null) return;

            view.SelectedSegment = (nint)value;
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }

        private void HandleValueChanged(object sender, EventArgs e)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged((int)view.SelectedSegment);
        }
    }
}
