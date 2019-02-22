// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Bindings.Core.Binding;
using Bindings.Core.Binding.Binders;
using Bindings.Core.Binding.BindingContext;
using Bindings.Core.Binding.Bindings.Target.Construction;
using Bindings.iOS.Binding.Target;
using Bindings.iOS.Binding.ValueConverters;
using UIKit;

namespace Bindings.iOS.Binding
{
    public class IosBindingBuilder
        : BindingBuilder
    {
        private readonly UnifiedTypesValueConverter _unifiedValueTypesConverter = new UnifiedTypesValueConverter();

        protected override void FillTargetFactories(ITargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterCustomBindingFactory<UIControl>(
                IosPropertyBinding.UIControl_TouchDown,
                view => new UiControlTargetBinding(view, IosPropertyBinding.UIControl_TouchDown));

            registry.RegisterCustomBindingFactory<UIControl>(
                IosPropertyBinding.UIControl_TouchDownRepeat,
                view => new UiControlTargetBinding(view, IosPropertyBinding.UIControl_TouchDownRepeat));
            
            registry.RegisterCustomBindingFactory<UIControl>(
                IosPropertyBinding.UIControl_TouchDragInside,
                view => new UiControlTargetBinding(view, IosPropertyBinding.UIControl_TouchDragInside));

            registry.RegisterCustomBindingFactory<UIControl>(
                IosPropertyBinding.UIControl_TouchUpInside,
                view => new UiControlTargetBinding(view, IosPropertyBinding.UIControl_TouchUpInside));

            registry.RegisterCustomBindingFactory<UIControl>(
                IosPropertyBinding.UIControl_ValueChanged,
                view => new UiControlTargetBinding(view, IosPropertyBinding.UIControl_ValueChanged));

            registry.RegisterCustomBindingFactory<UIControl>(
                IosPropertyBinding.UIControl_PrimaryActionTriggered,
                view => new UiControlTargetBinding(view, IosPropertyBinding.UIControl_PrimaryActionTriggered));

            registry.RegisterCustomBindingFactory<UIControl>(
                IosPropertyBinding.UIControl_EditingDidBegin,
                view => new UiControlTargetBinding(view, IosPropertyBinding.UIControl_EditingDidBegin));

            registry.RegisterCustomBindingFactory<UIControl>(
                IosPropertyBinding.UIControl_EditingChanged,
                view => new UiControlTargetBinding(view, IosPropertyBinding.UIControl_EditingChanged));

            registry.RegisterCustomBindingFactory<UIControl>(
                IosPropertyBinding.UIControl_EditingDidEnd,
                view => new UiControlTargetBinding(view, IosPropertyBinding.UIControl_EditingDidEnd));

            registry.RegisterCustomBindingFactory<UIControl>(
                IosPropertyBinding.UIControl_EditingDidEndOnExit,
                view => new UiControlTargetBinding(view, IosPropertyBinding.UIControl_EditingDidEndOnExit));

            registry.RegisterCustomBindingFactory<UIControl>(
                IosPropertyBinding.UIControl_AllTouchEvents,
                view => new UiControlTargetBinding(view, IosPropertyBinding.UIControl_AllTouchEvents));

            registry.RegisterCustomBindingFactory<UIControl>(
                IosPropertyBinding.UIControl_AllEditingEvents,
                view => new UiControlTargetBinding(view, IosPropertyBinding.UIControl_AllEditingEvents));
            
            registry.RegisterCustomBindingFactory<UIControl>(
                IosPropertyBinding.UIControl_AllEvents,
                view => new UiControlTargetBinding(view, IosPropertyBinding.UIControl_AllEvents));

            registry.RegisterCustomBindingFactory<UIView>(
                IosPropertyBinding.UIView_Visible,
                view =>   new UiViewVisibleTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIActivityIndicatorView>(
                IosPropertyBinding.UIActivityIndicatorView_Hidden,
                activityIndicator => new UiActivityIndicatorViewHiddenTargetBinding(activityIndicator));

            registry.RegisterCustomBindingFactory<UIView>(
                IosPropertyBinding.UIView_Hidden,
                view => new UiViewHiddenTargetBinding(view));

            registry.RegisterPropertyInfoBindingFactory(
                typeof(UiSliderValueTargetBinding),
                typeof(UISlider),
                IosPropertyBinding.UISlider_Value);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(UiStepperValueTargetBinding),
                typeof(UIStepper),
                IosPropertyBinding.UIStepper_Value);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(UiPageControlCurrentPageTargetBinding),
                typeof(UIPageControl),
                IosPropertyBinding.UIPageControl_CurrentPage);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(UiSegmentedControlSelectedSegmentTargetBinding),
                typeof(UISegmentedControl),
                IosPropertyBinding.UISegmentedControl_SelectedSegment);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(UiDatePickerDateTargetBinding),
                typeof(UIDatePicker),
                IosPropertyBinding.UIDatePicker_Date);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(UiDatePickerMinMaxTargetBinding),
                typeof(UIDatePicker),
                IosPropertyBinding.UIDatePicker_MinimumDate);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(UiDatePickerMinMaxTargetBinding),
                typeof(UIDatePicker),
                IosPropertyBinding.UIDatePicker_MaximumDate);

            registry.RegisterCustomBindingFactory<UIDatePicker>(
                IosPropertyBinding.UIDatePicker_Time,
                view => new UiDatePickerTimeTargetBinding(view, typeof(UIDatePicker).GetProperty(IosPropertyBinding.UIDatePicker_Date)));

            registry.RegisterPropertyInfoBindingFactory(
                typeof(UiDatePickerCountDownDurationTargetBinding),
                typeof(UIDatePicker),
                IosPropertyBinding.UIDatePicker_CountDownDuration);

            registry.RegisterCustomBindingFactory<UITextField>(
                IosPropertyBinding.UITextField_ShouldReturn,
                textField => new UiTextFieldShouldReturnTargetBinding(textField));

            registry.RegisterCustomBindingFactory<UILabel>(
                IosPropertyBinding.UILabel_Text,
                view => new UiLabelTextTargetBinding(view));

            registry.RegisterCustomBindingFactory<UITextField>(
                IosPropertyBinding.UITextField_Text,
                view => new UiTextFieldTextTargetBinding(view));

            registry.RegisterCustomBindingFactory<UITextView>(
                IosPropertyBinding.UITextView_Text,
                view => new UiTextViewTextTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIView>(
                IosPropertyBinding.UIView_LayerBorderWidth,
                view => new UiViewLayerBorderWidthTargetBinding(view));

            registry.RegisterCustomBindingFactory<UISwitch>(
                IosPropertyBinding.UISwitch_On,
                uiSwitch => new UiSwitchOnTargetBinding(uiSwitch));

            registry.RegisterPropertyInfoBindingFactory(
                typeof(UiSearchBarTextTargetBinding),
                typeof(UISearchBar),
                IosPropertyBinding.UISearchBar_Text);

            registry.RegisterCustomBindingFactory<UIButton>(
                IosPropertyBinding.UIButton_Title,
                button => new UiButtonTitleTargetBinding(button));

            registry.RegisterCustomBindingFactory<UIButton>(
                IosPropertyBinding.UIButton_DisabledTitle,
                button => new UiButtonTitleTargetBinding(button, UIControlState.Disabled));

            registry.RegisterCustomBindingFactory<UIButton>(
                IosPropertyBinding.UIButton_HighlightedTitle,
                button => new UiButtonTitleTargetBinding(button, UIControlState.Highlighted));

            registry.RegisterCustomBindingFactory<UIButton>(
                IosPropertyBinding.UIButton_SelectedTitle,
                button => new UiButtonTitleTargetBinding(button, UIControlState.Selected));

            registry.RegisterCustomBindingFactory<UIView>(
                IosPropertyBinding.UIView_Tap,
                view => new UiViewTapTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIView>(
                IosPropertyBinding.UIView_DoubleTap,
                view => new UiViewTapTargetBinding(view, 2));

            registry.RegisterCustomBindingFactory<UIView>(
                IosPropertyBinding.UIView_TwoFingerTap,
                view => new UiViewTapTargetBinding(view, 1, 2));

            registry.RegisterCustomBindingFactory<UITextField>(
                IosPropertyBinding.UITextField_TextFocus,
                textField => new UiTextFieldTextFocusTargetBinding(textField));

            registry.RegisterCustomBindingFactory<UIBarButtonItem>(
                IosPropertyBinding.UIBarButtonItem_Clicked,
                view => new UiBarButtonItemTargetBinding(view));
        }

        protected override void FillAutoValueConverters(IAutoValueConverters autoValueConverters)
        {
            base.FillAutoValueConverters(autoValueConverters);

            //register converter for xamarin unified types
            foreach (var (key, value) in UnifiedTypesValueConverter.UnifiedTypeConversions)
                autoValueConverters.Register(key, value, _unifiedValueTypesConverter);
        }

        protected override void FillDefaultBindingNames(IBindingNameRegistry registry)
        {
            base.FillDefaultBindingNames(registry);

            registry.AddOrOverwrite(typeof(UIButton), IosPropertyBinding.UIControl_TouchUpInside);
            registry.AddOrOverwrite(typeof(UIBarButtonItem), nameof(UIBarButtonItem.Clicked));
            registry.AddOrOverwrite(typeof(UISearchBar), IosPropertyBinding.UISearchBar_Text);
            registry.AddOrOverwrite(typeof(UITextField), IosPropertyBinding.UITextField_Text);
            registry.AddOrOverwrite(typeof(UITextView), IosPropertyBinding.UITextView_Text);
            registry.AddOrOverwrite(typeof(UILabel), IosPropertyBinding.UILabel_Text);
            registry.AddOrOverwrite(typeof(UIImageView), nameof(UIImageView.Image));
            registry.AddOrOverwrite(typeof(UIDatePicker), IosPropertyBinding.UIDatePicker_Date);
            registry.AddOrOverwrite(typeof(UISlider), IosPropertyBinding.UISlider_Value);
            registry.AddOrOverwrite(typeof(UISwitch), IosPropertyBinding.UISwitch_On);
            registry.AddOrOverwrite(typeof(UIProgressView), nameof(UIProgressView.Progress));
            registry.AddOrOverwrite(typeof(UISegmentedControl), IosPropertyBinding.UISegmentedControl_SelectedSegment);
            registry.AddOrOverwrite(typeof(UIActivityIndicatorView), IosPropertyBinding.UIActivityIndicatorView_Hidden);
        }
    }
}
