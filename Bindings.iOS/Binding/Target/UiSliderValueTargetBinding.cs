// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Bindings.Core;
using Bindings.Core.Binding;
using Bindings.Core.Binding.Bindings.Target;
using Bindings.Core.Logging;
using Bindings.Core.WeakSubscription;
using UIKit;

namespace Bindings.iOS.Binding.Target
{
    public class UiSliderValueTargetBinding : PropertyInfoTargetBinding<UISlider>
    {
        private IDisposable _subscription;

        public UiSliderValueTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as UISlider;
            if (view == null) return;

            view.Value = (float)value;
        }

        private void HandleSliderValueChanged(object sender, EventArgs e)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged(view.Value);
        }

        public override BindingMode DefaultMode => BindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var slider = View;
            if (slider == null)
            {
                Log.Error( "Error - UISlider is null in MvxUISliderValueTargetBinding");
                return;
            }

            _subscription = slider.WeakSubscribe(nameof(slider.ValueChanged), HandleSliderValueChanged);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing)
                return;

            _subscription?.Dispose();
            _subscription = null;
        }
    }
}
