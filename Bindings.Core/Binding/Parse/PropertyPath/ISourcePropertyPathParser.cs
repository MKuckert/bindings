// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Bindings.Core.Binding.Parse.PropertyPath.PropertyTokens;

namespace Bindings.Core.Binding.Parse.PropertyPath
{
    public interface ISourcePropertyPathParser
    {
        IList<PropertyToken> Parse(string textToParse);
    }
}