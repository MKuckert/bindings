// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Bindings.Core.Logging;

namespace Bindings.Core.Base
{
    public abstract class Applicable
        : IApplicable
    {
        private bool _finalizerSuppressed;

        ~Applicable()
        {
            Log.Trace("Finaliser called on {0} - suggests that  Apply() was never called", GetType().Name);
        }

        protected void SuppressFinalizer()
        {
            if (_finalizerSuppressed)
                return;

            _finalizerSuppressed = true;
            GC.SuppressFinalize(this);
        }

        public virtual void Apply()
        {
            SuppressFinalizer();
        }
    }
}
