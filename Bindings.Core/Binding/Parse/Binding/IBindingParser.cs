// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Bindings.Core.Binding.Parse.Binding
{
    public interface IBindingParser
    {
        bool TryParseBindingDescription(string text, out SerializableBindingDescription requestedDescription);

        bool TryParseBindingSpecification(string text, out SerializableBindingSpecification requestedBindings);
    }
}