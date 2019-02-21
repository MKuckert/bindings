// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Bindings.Core.Binding.Bindings;

namespace Bindings.Core.Binding.Binders
{
    public class FromTextBinder
        : IBinder
    {
        public IEnumerable<IUpdateableBinding> Bind(object source, object target, string bindingText)
        {
            var bindingDescriptions = BindingSingletonCache.Instance.BindingDescriptionParser.Parse(bindingText);
            return Bind(source, target, bindingDescriptions);
        }

        public IEnumerable<IUpdateableBinding> Bind(object source, object target,
                                                       IEnumerable<BindingDescription> bindingDescriptions)
        {
            if (bindingDescriptions == null)
                return new IUpdateableBinding[0];

            return
                bindingDescriptions.Select(description => BindSingle(new BindingRequest(source, target, description)));
        }

        public IUpdateableBinding BindSingle(BindingRequest bindingRequest)
        {
            return new FullBinding(bindingRequest);
        }
    }
}