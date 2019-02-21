// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using Bindings.Core.Binding;
using Bindings.Core.Binding.Bindings.Target;
using UIKit;

namespace Bindings.iOS.Binding.Target
{
    public class UiTextFieldShouldReturnTargetBinding : TargetBinding
    {
        private ICommand _command;

        protected UITextField View => Target as UITextField;

        public UiTextFieldShouldReturnTargetBinding(UITextField target)
            : base(target)
        {
            target.ShouldReturn = HandleShouldReturn;
        }

        private bool HandleShouldReturn(UITextField textField)
        {
            if (_command == null)
                return false;

            var text = textField.Text;
            if (!_command.CanExecute(text))
                return false;

            textField.ResignFirstResponder();
            _command.Execute(text);
            return true;
        }

        public override BindingMode DefaultMode => BindingMode.OneWay;

        public override void SetValue(object value)
        {
            var command = value as ICommand;
            _command = command;
        }

        public override Type TargetType => typeof(ICommand);

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            var editText = View;
            if (editText == null) return;

            editText.ShouldReturn = null;
        }
    }
}
