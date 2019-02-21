// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using UIKit;

namespace Bindings.iOS.Binding
{
    public static class IosPropertyBindingExtensions
    {
        public static string BindTouchUpInside(this UIControl uiControl)
            => IosPropertyBinding.UIControl_TouchUpInside;

        public static string BindValueChanged(this UIControl uiControl)
            => IosPropertyBinding.UIControl_ValueChanged;

        public static string BindTouchDown(this UIControl uiControl)
            => IosPropertyBinding.UIControl_TouchDown;

        public static string BindTouchDownRepeat(this UIControl uiControl)
            => IosPropertyBinding.UIControl_TouchDownRepeat;

        public static string BindTouchDragInside(this UIControl uiControl)
            => IosPropertyBinding.UIControl_TouchDragInside;

        public static string BindPrimaryActionTriggered(this UIControl uiControl)
            => IosPropertyBinding.UIControl_PrimaryActionTriggered;

        public static string BindEditingDidBegin(this UIControl uiControl)
            => IosPropertyBinding.UIControl_EditingDidBegin;

        public static string BindEditingChanged(this UIControl uiControl)
            => IosPropertyBinding.UIControl_EditingChanged;

        public static string BindEditingDidEnd(this UIControl uiControl)
            => IosPropertyBinding.UIControl_EditingDidEnd;

        public static string BindEditingDidEndOnExit(this UIControl uiControl)
            => IosPropertyBinding.UIControl_EditingDidEndOnExit;

        public static string BindAllTouchEvents(this UIControl uiControl)
            => IosPropertyBinding.UIControl_AllTouchEvents;

        public static string BindAllEditingEvents(this UIControl uiControl)
            => IosPropertyBinding.UIControl_AllEditingEvents;

        public static string BindAllEvents(this UIControl uiControl)
            => IosPropertyBinding.UIControl_AllEvents;

        public static string BindVisible(this UIView uiView)
            => IosPropertyBinding.UIView_Visible;

        public static string BindHidden(this UIActivityIndicatorView uiActivityIndicatorView)
             => IosPropertyBinding.UIActivityIndicatorView_Hidden;

        public static string BindHidden(this UIView uiView)
            => IosPropertyBinding.UIView_Hidden;

        public static string BindValue(this UISlider uiSlider)
            => IosPropertyBinding.UISlider_Value;

        public static string BindValue(this UIStepper uiStepper)
            => IosPropertyBinding.UIStepper_Value;

        public static string BindSelectedSegment(this UISegmentedControl uiSegmentedControl)
            => IosPropertyBinding.UISegmentedControl_SelectedSegment;

        public static string BindDate(this UIDatePicker uiDatePicker)
            => IosPropertyBinding.UIDatePicker_Date;
        
        public static string BindCountDownDuration(this UIDatePicker uiDatePicker)
            => IosPropertyBinding.UIDatePicker_CountDownDuration;

        public static string BindShouldReturn(this UITextField uiTextField)
            => IosPropertyBinding.UITextField_ShouldReturn;

        public static string BindTime(this UIDatePicker uiDatePicker)
            => IosPropertyBinding.UIDatePicker_Time;

        public static string BindText(this UILabel uiLabel)
            => IosPropertyBinding.UILabel_Text;

        public static string BindText(this UITextField uiTextField)
            => IosPropertyBinding.UITextField_Text;

        public static string BindText(this UITextView uiTextView)
            => IosPropertyBinding.UITextView_Text;

        public static string BindLayerBorderWidth(this UIView uiView)
            => IosPropertyBinding.UIView_LayerBorderWidth;

        public static string BindOn(this UISwitch uiSwitch)
            => IosPropertyBinding.UISwitch_On;

        public static string BindText(this UISearchBar uiSearchBar)
            => IosPropertyBinding.UISearchBar_Text;

        public static string BindTitle(this UIButton uiButton)
            => IosPropertyBinding.UIButton_Title;

        public static string BindDisabledTitle(this UIButton uiButton)
            => IosPropertyBinding.UIButton_DisabledTitle;

        public static string BindHighlightedTitle(this UIButton uiButton)
            => IosPropertyBinding.UIButton_HighlightedTitle;

        public static string BindSelectedTitle(this UIButton uiButton)
            => IosPropertyBinding.UIButton_SelectedTitle;

        public static string BindTap(this UIView uiView)
            => IosPropertyBinding.UIView_Tap;

        public static string BindDoubleTap(this UIView uiView)
            => IosPropertyBinding.UIView_DoubleTap;

        public static string BindTwoFingerTap(this UIView uiView)
            => IosPropertyBinding.UIView_TwoFingerTap;

        public static string BindTextFocus(this UIView uiView)
            => IosPropertyBinding.UITextField_TextFocus;

        public static string BindClicked(this UIBarButtonItem uiBarButtonItem)
            => IosPropertyBinding.UIBarButtonItem_Clicked;
    }
}
