// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Bindings.Core.IoC;
using Bindings.Core.Logging;

namespace Bindings.Core.Binding.BindingContext
{
    public class BindingContextStackRegistration<TBindingContext>
        : IDisposable
    {
        private static IBindingContextStack<TBindingContext> Stack => IoCProvider.Instance.Resolve<IBindingContextStack<TBindingContext>>();

        public BindingContextStackRegistration(TBindingContext toRegister)
        {
            Stack.Push(toRegister);
        }

        ~BindingContextStackRegistration()
        {
            Log.Error($"You should always Dispose of {GetType().Name}");
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stack.Pop();
            }
        }
    }
}