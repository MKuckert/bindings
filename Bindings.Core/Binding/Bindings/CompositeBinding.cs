// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

namespace Bindings.Core.Binding.Bindings
{
    public class CompositeBinding : Binding
    {
        private readonly List<IBinding> _bindings;

        public CompositeBinding(params IBinding[] args)
        {
            _bindings = args.ToList();
        }

        public void Add(params IBinding[] args)
        {
            _bindings.AddRange(args);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                foreach (var mvxBinding in _bindings)
                {
                    mvxBinding.Dispose();
                }
                _bindings.Clear();
            }
            base.Dispose(isDisposing);
        }
    }
}