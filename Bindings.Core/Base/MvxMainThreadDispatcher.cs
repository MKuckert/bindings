// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using MvvmCross.Binding;

namespace MvvmCross.Base
{
    public abstract class MvxMainThreadDispatcher : MvxSingleton<IMvxMainThreadDispatcher>, IMvxMainThreadDispatcher
    {
        public static void ExceptionMaskedAction(Action action, bool maskExceptions)
        {
            try
            {
                action();
            }
            catch (TargetInvocationException exception)
            {
                MvxBindingLog.Trace("Exception throw when invoking action via dispatcher", exception);
                if (maskExceptions)
                    MvxBindingLog.Warning("TargetInvocateException masked " + exception.InnerException);
                else
                    throw exception;
            }
            catch (Exception exception)
            {
                MvxBindingLog.Trace("Exception throw when invoking action via dispatcher", exception);
                if (maskExceptions)
                    MvxBindingLog.Warning("Exception masked " + exception);
                else
                    throw exception;
            }
        }

        public abstract bool RequestMainThreadAction(Action action, bool maskExceptions = true);

        public abstract bool IsOnMainThread { get; }
    }
}
