﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace Bindings.Core.Base
{
    public abstract class ValueEventArgs<T>
        : EventArgs
    {
        protected ValueEventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
