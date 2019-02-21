// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace Bindings.Core.Binding.Bindings.Target
{
    public class TargetChangedEventArgs
        : EventArgs
    {
        public TargetChangedEventArgs(object value)
        {
            Value = value;
        }

        public object Value { get; private set; }
    }
}