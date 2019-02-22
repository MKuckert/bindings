// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Bindings.Core;
using Bindings.Core.Binding;
using Bindings.Core.Binding.Bindings.Target;
using Bindings.Core.Logging;
using Bindings.Core.WeakSubscription;
using UIKit;

namespace Bindings.iOS.Binding.Target
{
    public class UiTextViewTextTargetBinding : ConvertingTargetBinding
    {
        private IDisposable _subscription;

        protected UITextView View => Target as UITextView;


        public UiTextViewTextTargetBinding(UITextView target)
            : base(target)
        {
        }

        private void EditTextOnChanged(object sender, NSTextStorageEventArgs eventArgs)
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
                                      "Error - UITextView is null in MvxUITextViewTextTargetBinding");
                return;
            }

			var textStorage = target.LayoutManager?.TextStorage;
			if (textStorage == null)
			{ 
			    Log.Error(
						  "Error - NSTextStorage of UITextView is null in MvxUITextViewTextTargetBinding");
				return;
			}

            _subscription = textStorage.WeakSubscribe<NSTextStorage, NSTextStorageEventArgs>(nameof(textStorage.DidProcessEditing), EditTextOnChanged);
        }

        public override Type TargetType => typeof(string);

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UITextView)target;
            if (view == null) return;

            view.Text = (string)value;
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
