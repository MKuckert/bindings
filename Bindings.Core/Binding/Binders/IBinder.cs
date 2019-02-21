// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Bindings.Core.Binding.Bindings;

namespace Bindings.Core.Binding.Binders
{
    public interface IBinder
    {
        IEnumerable<IUpdateableBinding> Bind(object source, object target, string bindingText);

        IEnumerable<IUpdateableBinding> Bind(object source, object target,
                                                IEnumerable<BindingDescription> bindingDescriptions);
    }
}