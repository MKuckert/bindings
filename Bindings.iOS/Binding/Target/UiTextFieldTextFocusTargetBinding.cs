// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Bindings.Core.Binding;
using Bindings.Core.Binding.Bindings.Target;
using Bindings.Core.WeakSubscription;
using UIKit;

namespace Bindings.iOS.Binding.Target
{
    public class UiTextFieldTextFocusTargetBinding : TargetBinding
    {
        private IDisposable _subscription;

        protected UITextField TextField => Target as UITextField;

        public override Type TargetType => typeof(string);

        public override BindingMode DefaultMode => BindingMode.TwoWay;

        public UiTextFieldTextFocusTargetBinding(object target)
            : base(target)
        {
        }

        public override void SetValue(object value)
        {
            if (TextField == null) return;

            value = value ?? string.Empty;
            TextField.Text = value.ToString();
        }

        public override void SubscribeToEvents()
        {
            var textField = TextField;
            if (TextField == null) return;

            _subscription = textField.WeakSubscribe(nameof(textField.EditingDidEnd), HandleLostFocus);
        }

        private void HandleLostFocus(object sender, EventArgs e)
        {
            var textField = TextField;
            if (textField == null) return;

            FireValueChanged(textField.Text);
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
