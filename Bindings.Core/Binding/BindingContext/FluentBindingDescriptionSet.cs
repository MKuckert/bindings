// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Bindings.Core.Base;
using Bindings.Core.Binding.Bindings;

namespace Bindings.Core.Binding.BindingContext
{
    public class FluentBindingDescriptionSet<TOwningTarget, TSource>
        : Applicable
        where TOwningTarget : class, IBindingContextOwner
    {
        private readonly List<IApplicable> _applicables = new List<IApplicable>();
        private readonly TOwningTarget _bindingContextOwner;

        public FluentBindingDescriptionSet(TOwningTarget bindingContextOwner)
        {
            _bindingContextOwner = bindingContextOwner;
        }

        public FluentBindingDescription<TOwningTarget, TSource> Bind()
        {
            var toReturn = new FluentBindingDescription<TOwningTarget, TSource>(_bindingContextOwner,
                                                                                   _bindingContextOwner);
            _applicables.Add(toReturn);
            return toReturn;
        }

        public FluentBindingDescription<TChildTarget, TSource> Bind<TChildTarget>(TChildTarget childTarget)
            where TChildTarget : class
        {
            var toReturn = new FluentBindingDescription<TChildTarget, TSource>(_bindingContextOwner, childTarget);
            _applicables.Add(toReturn);
            return toReturn;
        }

        public FluentBindingDescription<TChildTarget, TSource> Bind<TChildTarget>(TChildTarget childTarget,
                                                                                     string bindingDescription)
            where TChildTarget : class
        {
            var toReturn = Bind(childTarget);
            toReturn.FullyDescribed(bindingDescription);
            return toReturn;
        }

        public FluentBindingDescription<TChildTarget, TSource> Bind<TChildTarget>(TChildTarget childTarget,
                                                                                     BindingDescription bindingDescription)
            where TChildTarget : class
        {
            var toReturn = Bind(childTarget);
            toReturn.FullyDescribed(bindingDescription);
            return toReturn;
        }

        public override void Apply()
        {
            foreach (var applicable in _applicables)
                applicable.Apply();
            base.Apply();
        }

        public void ApplyWithClearBindingKey(object clearBindingKey)
        {
            foreach (var applicable in _applicables)
            {
                if (applicable is IBaseFluentBindingDescription fluentBindingDescription)
                {
                    fluentBindingDescription.ClearBindingKey = clearBindingKey;
                }
                else
                {
                    BindingLog.Warning("Fluent binding description must implement {0} in order to add {1}",
                        nameof(IBaseFluentBindingDescription),
                        nameof(IBaseFluentBindingDescription.ClearBindingKey));
                }

                applicable.Apply();
            }

            base.Apply();
        }
    }
}
