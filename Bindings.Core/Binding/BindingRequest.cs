// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Bindings.Core.Binding.Bindings;

namespace Bindings.Core.Binding
{
    public class BindingRequest
    {
        public BindingRequest()
        {
        }

        public BindingRequest(object source, object target, BindingDescription description)
        {
            Target = target;
            Source = source;
            Description = description;
        }

        public object Target { get; set; }
        public object Source { get; set; }
        public BindingDescription Description { get; set; }
    }
}