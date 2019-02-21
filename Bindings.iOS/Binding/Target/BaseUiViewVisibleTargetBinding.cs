// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Bindings.Core.Binding;
using Bindings.Core.Binding.Bindings.Target;
using UIKit;

namespace Bindings.iOS.Binding.Target
{
    public abstract class BaseUiViewVisibleTargetBinding : ConvertingTargetBinding
    {
        protected UIView View => (UIView)Target;

        protected BaseUiViewVisibleTargetBinding(UIView target)
            : base(target)
        {
        }

        public override BindingMode DefaultMode => BindingMode.OneWay;

        public override Type TargetType => typeof(bool);
    }
}
