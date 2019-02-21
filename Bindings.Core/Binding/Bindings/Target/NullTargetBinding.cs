// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace Bindings.Core.Binding.Bindings.Target
{
    public class NullTargetBinding : TargetBinding
    {
        public NullTargetBinding()
            : base(null)
        {
        }

        public override BindingMode DefaultMode => BindingMode.OneTime;

        public override Type TargetType => typeof(object);

        public override void SetValue(object value)
        {
            // ignored
        }
    }
}