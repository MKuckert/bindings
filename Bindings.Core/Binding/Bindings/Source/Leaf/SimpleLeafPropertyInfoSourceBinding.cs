// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace Bindings.Core.Binding.Bindings.Source.Leaf
{
    public class SimpleLeafPropertyInfoSourceBinding : LeafPropertyInfoSourceBinding
    {
        public SimpleLeafPropertyInfoSourceBinding(object source, PropertyInfo propertyInfo)
            : base(source, propertyInfo)
        {
        }

        protected override object[] PropertyIndexParameters()
        {
            return null;
        }
    }
}