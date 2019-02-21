// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Bindings.Core.Binding.Parse.PropertyPath.PropertyTokens;

namespace Bindings.Core.Binding.Bindings.Source.Leaf
{
    public class IndexerLeafPropertyInfoSourceBinding : LeafPropertyInfoSourceBinding
    {
        private readonly object _key;

        public IndexerLeafPropertyInfoSourceBinding(object source, PropertyInfo itemPropertyInfo, IndexerPropertyToken indexToken)
            : base(source, itemPropertyInfo)
        {
            _key = indexToken.Key;
        }

        protected override object[] PropertyIndexParameters()
        {
            return new[] { _key };
        }
    }
}