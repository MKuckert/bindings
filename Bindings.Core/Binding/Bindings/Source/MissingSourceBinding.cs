// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Bindings.Core.Converters;

namespace Bindings.Core.Binding.Bindings.Source
{
    public class MissingSourceBinding : SourceBinding
    {
        public MissingSourceBinding(object source) : base(source)
        {
        }

        public override void SetValue(object value)
        {
            // nothing we can do here - binding is missing
        }

        public override Type SourceType => typeof(object);

        public override object GetValue()
        {
            // binding is missing so return 'unset value'
            return BindingConstant.UnsetValue;
        }
    }
}