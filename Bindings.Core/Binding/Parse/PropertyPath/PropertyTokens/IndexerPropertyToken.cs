// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Bindings.Core.Binding.Parse.PropertyPath.PropertyTokens
{
    public class IndexerPropertyToken : PropertyToken
    {
        protected IndexerPropertyToken(object key)
        {
            Key = key;
        }

        public object Key { get; }

        public override string ToString()
        {
            return "IndexedProperty:" + (Key == null ? "null" : Key.ToString());
        }
    }

    public class IndexerPropertyToken<T> : IndexerPropertyToken
    {
        protected IndexerPropertyToken(T key)
            : base(key)
        {
        }

        public new T Key => (T)base.Key;
    }
}