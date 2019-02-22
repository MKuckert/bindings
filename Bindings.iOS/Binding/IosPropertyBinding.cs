// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

// ReSharper disable InconsistentNaming

using UIKit;

namespace Bindings.iOS.Binding
{
    internal static class IosPropertyBinding
    {
        public const string UIControl_TouchDown = nameof(UIControl.TouchDown);
        public const string UIControl_TouchDownRepeat = nameof(UIControl.TouchDownRepeat);
        public const string UIControl_TouchDragInside = nameof(UIControl.TouchDragInside);
        public const string UIControl_TouchUpInside = nameof(UIControl.TouchUpInside);
        public const string UIControl_ValueChanged = nameof(UIControl.ValueChanged);
        public const string UIControl_PrimaryActionTriggered = nameof(UIControl.PrimaryActionTriggered);
        public const string UIControl_EditingDidBegin = nameof(UIControl.EditingDidBegin);
        public const string UIControl_EditingChanged = nameof(UIControl.EditingChanged);
        public const string UIControl_EditingDidEnd = nameof(UIControl.EditingDidEnd);
        public const string UIControl_EditingDidEndOnExit = nameof(UIControl.EditingDidEndOnExit);
        public const string UIControl_AllTouchEvents = nameof(UIControl.AllTouchEvents);
        public const string UIControl_AllEditingEvents = nameof(UIControl.AllEditingEvents);
        public const string UIControl_AllEvents = nameof(UIControl.AllEvents);
        public const string UIView_Visible = "Visible";
        public const string UIActivityIndicatorView_Hidden = nameof(UIActivityIndicatorView.Hidden);
        public const string UIView_Hidden = nameof(UIView.Hidden);
        public const string UISlider_Value = nameof(UISlider.Value);
        public const string UIStepper_Value = nameof(UIStepper.Value);
        public const string UIPageControl_CurrentPage = nameof(UIPageControl.CurrentPage);
        public const string UISegmentedControl_SelectedSegment = nameof(UISegmentedControl.SelectedSegment);
        public const string UIDatePicker_Date = nameof(UIDatePicker.Date);
        public const string UIDatePicker_MaximumDate = nameof(UIDatePicker.MaximumDate);
        public const string UIDatePicker_MinimumDate = nameof(UIDatePicker.MinimumDate);
        public const string UIDatePicker_Time = "Time";
        public const string UIDatePicker_CountDownDuration = nameof(UIDatePicker.CountDownDuration);
        public const string UITextField_ShouldReturn = nameof(UITextField.ShouldReturn);
        public const string UILabel_Text = nameof(UILabel.Text);
        public const string UITextField_Text = nameof(UITextField.Text);
        public const string UITextView_Text = nameof(UITextView.Text);
        public const string UIView_LayerBorderWidth = "LayerBorderWidth";
        public const string UISwitch_On = nameof(UISwitch.On);
        public const string UISearchBar_Text = nameof(UISearchBar.Text);
        public const string UIButton_Title = nameof(UIButton.Title);
        public const string UIButton_DisabledTitle = "DisabledTitle";
        public const string UIButton_HighlightedTitle = "HighlightedTitle";
        public const string UIButton_SelectedTitle = "SelectedTitle";
        public const string UIView_Tap = "Tap";
        public const string UIView_DoubleTap = "DoubleTap";
        public const string UIView_TwoFingerTap = "TwoFingerTap";
        public const string UITextField_TextFocus = "TextFocus";
        public const string UIBarButtonItem_Clicked = nameof(UIBarButtonItem.Clicked);
    }
}
