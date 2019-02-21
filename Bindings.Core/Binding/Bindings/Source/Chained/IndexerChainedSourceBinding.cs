// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reflection;
using Bindings.Core.Binding.Parse.PropertyPath.PropertyTokens;

namespace Bindings.Core.Binding.Bindings.Source.Chained
{
    public class IndexerChainedSourceBinding
        : ChainedSourceBinding
    {
        private readonly IndexerPropertyToken _indexerPropertyToken;

        public IndexerChainedSourceBinding(object source, PropertyInfo itemPropertyInfo, IndexerPropertyToken indexerPropertyToken,
                                                  IList<PropertyToken> childTokens)
            : base(source, itemPropertyInfo, childTokens)
        {
            _indexerPropertyToken = indexerPropertyToken;
            UpdateChildBinding();
        }

        protected override object[] PropertyIndexParameters()
        {
            return new[] { _indexerPropertyToken.Key };
        }
    }
}