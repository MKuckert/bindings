// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Bindings.Core.Binding.Bindings;

namespace Bindings.Core.Binding.Binders
{
    public interface IBindingDescriptionParser
    {
        IEnumerable<BindingDescription> Parse(string text);

        BindingDescription ParseSingle(string text);
    }
}