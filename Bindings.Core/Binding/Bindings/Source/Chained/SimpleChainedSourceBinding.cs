// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reflection;
using Bindings.Core.Binding.Parse.PropertyPath.PropertyTokens;

namespace Bindings.Core.Binding.Bindings.Source.Chained
{
    public class SimpleChainedSourceBinding
        : ChainedSourceBinding
    {
        public SimpleChainedSourceBinding(
            object source,
            PropertyInfo propertyInfo,
            IList<PropertyToken> childTokens)
            : base(source, propertyInfo, childTokens)
        {
            UpdateChildBinding();
        }

        protected override object[] PropertyIndexParameters()
        {
            return null;
        }
    }
}