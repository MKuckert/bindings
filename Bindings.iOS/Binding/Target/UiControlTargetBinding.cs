// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using Bindings.Core.Binding;
using Bindings.Core.Binding.Bindings.Target;
using Bindings.Core.WeakSubscription;
using UIKit;

namespace Bindings.iOS.Binding.Target
{
    public class UiControlTargetBinding : ConvertingTargetBinding
    {
        private ICommand _command;
        private IDisposable _canExecuteSubscription;
        private IDisposable _controlEventSubscription;

        private readonly string _controlEvent;
        private readonly EventHandler<EventArgs> _canExecuteEventHandler;
        
        protected UIControl Control => Target as UIControl;

        public UiControlTargetBinding(UIControl control, string controlEvent)
            : base(control)
        {
            _controlEvent = controlEvent;

            if (control == null)
            {
                BindingLog.Error( "Error - UIControl is null in MvxUIControlTargetBinding");
            }
            else
            {
                AddHandler(control);    
            }

            _canExecuteEventHandler = OnCanExecuteChanged;
        }

        private void ControlEvent(object sender, EventArgs eventArgs)
        {
            if (_command == null) return;

            if (!_command.CanExecute(null)) return;

            _command.Execute(null);
        }

        public override BindingMode DefaultMode => BindingMode.OneWay;

        public override Type TargetType => typeof(ICommand);

        protected override void SetValueImpl(object target, object value)
        {
            _canExecuteSubscription?.Dispose();
            _canExecuteSubscription = null;

            _command = value as ICommand;
            if (_command != null)
            {
                _canExecuteSubscription = _command.WeakSubscribe(_canExecuteEventHandler);
            }

            RefreshEnabledState();
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            RemoveHandler();
            _canExecuteSubscription?.Dispose();
            _canExecuteSubscription = null;
        }

        private void RefreshEnabledState()
        {
            var view = Control;
            if (view == null) return;

            view.Enabled = _command?.CanExecute(null) ?? false;
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            RefreshEnabledState();
        }

        private void AddHandler(UIControl control)
        {
            switch (_controlEvent)
            {
                case IosPropertyBinding.UIControl_TouchDown:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.TouchDown), ControlEvent);
                    break;
                case IosPropertyBinding.UIControl_TouchDownRepeat:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.TouchDownRepeat), ControlEvent);
                    break;
                case IosPropertyBinding.UIControl_TouchDragInside:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.TouchDragInside), ControlEvent);
                    break;
                case IosPropertyBinding.UIControl_TouchUpInside:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.TouchUpInside), ControlEvent);
                    break;
                case IosPropertyBinding.UIControl_ValueChanged:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.ValueChanged), ControlEvent);
                    break;
                case IosPropertyBinding.UIControl_PrimaryActionTriggered:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.PrimaryActionTriggered), ControlEvent);
                    break;
                case IosPropertyBinding.UIControl_EditingDidBegin:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.EditingDidBegin), ControlEvent);
                    break;
                case IosPropertyBinding.UIControl_EditingChanged:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.EditingChanged), ControlEvent);
                    break;
                case IosPropertyBinding.UIControl_EditingDidEnd:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.EditingDidEnd), ControlEvent);
                    break;
                case IosPropertyBinding.UIControl_EditingDidEndOnExit:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.EditingDidEndOnExit), ControlEvent);
                    break;
                case IosPropertyBinding.UIControl_AllTouchEvents:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.AllTouchEvents), ControlEvent);
                    break;
                case IosPropertyBinding.UIControl_AllEditingEvents:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.AllEditingEvents), ControlEvent);
                    break;
                case IosPropertyBinding.UIControl_AllEvents:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.AllEvents), ControlEvent);
                    break;
                default:
                    BindingLog.Error( "Error - Invalid controlEvent in MvxUIControlTargetBinding");
                    break;
            }
        }

        private void RemoveHandler()
        {
            switch (_controlEvent)
            {
                case IosPropertyBinding.UIControl_TouchDown:
                case IosPropertyBinding.UIControl_TouchDownRepeat:
                case IosPropertyBinding.UIControl_TouchDragInside:
                case IosPropertyBinding.UIControl_TouchUpInside:
                case IosPropertyBinding.UIControl_ValueChanged:
                case IosPropertyBinding.UIControl_PrimaryActionTriggered:
                case IosPropertyBinding.UIControl_EditingDidBegin:
                case IosPropertyBinding.UIControl_EditingChanged:
                case IosPropertyBinding.UIControl_EditingDidEnd:
                case IosPropertyBinding.UIControl_EditingDidEndOnExit:
                case IosPropertyBinding.UIControl_AllTouchEvents:
                case IosPropertyBinding.UIControl_AllEditingEvents:
                case IosPropertyBinding.UIControl_AllEvents:
                    _controlEventSubscription?.Dispose();
                    break;
                default:
                    BindingLog.Error( "Error - Invalid controlEvent in MvxUIControlTargetBinding");
                    break;
            }
        }
    }
}
