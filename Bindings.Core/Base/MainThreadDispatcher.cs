// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Bindings.Core.Binding;

namespace Bindings.Core.Base
{
    public abstract class MainThreadDispatcher : Singleton<IMainThreadDispatcher>, IMainThreadDispatcher
    {
        public static void ExceptionMaskedAction(Action action, bool maskExceptions)
        {
            try
            {
                action();
            }
            catch (TargetInvocationException exception)
            {
                BindingLog.Trace("Exception throw when invoking action via dispatcher", exception);
                if (maskExceptions)
                    BindingLog.Warning("TargetInvocateException masked " + exception.InnerException);
                else
                    throw;
            }
            catch (Exception exception)
            {
                BindingLog.Trace("Exception throw when invoking action via dispatcher", exception);
                if (maskExceptions)
                    BindingLog.Warning("Exception masked " + exception);
                else
                    throw;
            }
        }

        public abstract bool RequestMainThreadAction(Action action, bool maskExceptions = true);

        public abstract bool IsOnMainThread { get; }
    }
}
