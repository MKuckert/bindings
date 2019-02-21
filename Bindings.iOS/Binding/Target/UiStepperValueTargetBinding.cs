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
    public class UiStepperValueTargetBinding : PropertyInfoTargetBinding<UIStepper>
    {
        private IDisposable _subscription;

        public UiStepperValueTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as UIStepper;
            if (view == null) return;

            view.Value = (double)value;
        }

        private void HandleValueChanged(object sender, EventArgs e)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged(view.Value);
        }

        public override BindingMode DefaultMode => BindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var stepper = View;
            if (stepper == null)
            {
                BindingLog.Error( "UIStepper is null in MvxUIStepperValueTargetBinding");
                return;
            }

            _subscription = stepper.WeakSubscribe(nameof(stepper.ValueChanged), HandleValueChanged);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }
    }
}
