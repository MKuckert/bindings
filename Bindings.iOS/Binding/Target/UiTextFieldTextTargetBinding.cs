// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Bindings.Core;
using Bindings.Core.Binding;
using Bindings.Core.Binding.Bindings.Target;
using Bindings.Core.Binding.Extensions;
using Bindings.Core.Logging;
using Bindings.Core.WeakSubscription;
using UIKit;

namespace Bindings.iOS.Binding.Target
{
    public class UiTextFieldTextTargetBinding : ConvertingTargetBinding, IEditableTextView
    {
        protected UITextField View => Target as UITextField;

        private IDisposable _subscriptionChanged;
        private IDisposable _subscriptionEndEditing;

        public UiTextFieldTextTargetBinding(UITextField target)
            : base(target)
        {
        }

        private void HandleEditTextValueChanged(object sender, EventArgs e)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged(view.Text);
        }

        public override BindingMode DefaultMode => BindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var target = View;
            if (target == null)
            {
                Log.Error(
                                      "Error - UITextField is null in MvxUITextFieldTextTargetBinding");
                return;
            }

            _subscriptionChanged = target.WeakSubscribe(nameof(target.EditingChanged), HandleEditTextValueChanged);
            _subscriptionEndEditing = target.WeakSubscribe(nameof(target.EditingDidEnd), HandleEditTextValueChanged);
        }

        public override Type TargetType => typeof(string);

        protected override bool ShouldSkipSetValueForViewSpecificReasons(object target, object value)
            => this.ShouldSkipSetValueAsHaveNearlyIdenticalNumericText(target, value);

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UITextField)target;
            if (view == null) return;

            view.Text = (string)value;
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscriptionChanged?.Dispose();
            _subscriptionChanged = null;

            _subscriptionEndEditing?.Dispose();
            _subscriptionEndEditing = null;
        }

        public string CurrentText
        {
            get
            {
                var view = View;
                return view?.Text;
            }
        }
    }
}
