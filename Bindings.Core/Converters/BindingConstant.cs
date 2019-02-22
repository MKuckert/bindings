// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Bindings.Core.Converters
{
    public class BindingConstant
    {
        public static readonly BindingConstant DoNothing = new BindingConstant(nameof(DoNothing));
        public static readonly BindingConstant UnsetValue = new BindingConstant(nameof(UnsetValue));

        private readonly string _debug;

        private BindingConstant(string debug)
        {
            _debug = debug;
        }

        public override string ToString()
        {
            return "Binding:" + _debug;
        }
    }
}
