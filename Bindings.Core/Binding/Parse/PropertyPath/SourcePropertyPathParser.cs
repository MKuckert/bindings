// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using Bindings.Core.Binding.Parse.PropertyPath.PropertyTokens;

namespace Bindings.Core.Binding.Parse.PropertyPath
{
    /// <summary>
    /// Stateless parser with global caching of tokens
    /// </summary>
    public class SourcePropertyPathParser : ISourcePropertyPathParser
    {
        private static readonly ConcurrentDictionary<int, IList<PropertyToken>> ParseCache = 
            new ConcurrentDictionary<int, IList<PropertyToken>>();

        public IList<PropertyToken> Parse(string textToParse)
        {
            textToParse = PropertyPathParser.MakeSafe(textToParse);
            var hash = textToParse.GetHashCode();
            IList<PropertyToken> list;
            if (ParseCache.TryGetValue(hash, out list))
                return list;

            var parser = new PropertyPathParser();
            var currentTokens = parser.Parse(textToParse);

            ParseCache.TryAdd(hash, currentTokens);
            return currentTokens;
        }
    }
}
