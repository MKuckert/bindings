// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Threading.Tasks;
using Bindings.Core.Logging;

namespace Bindings.Core.Base
{
    public abstract class MainThreadAsyncDispatcher  : Singleton<IMainThreadAsyncDispatcher>, IMainThreadAsyncDispatcher
    {
        protected abstract bool RequestMainThreadAction(Action action, bool maskExceptions = true);
        
        public Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true)
        {
            var completion = new TaskCompletionSource<bool>();
            var syncAction = new Action(() =>
            {
                action();
                completion.SetResult(true);
            });
            RequestMainThreadAction(syncAction, maskExceptions);

            // If we're already on main thread, then the action will
            // have already completed at this point, so can just return
            if (completion.Task.IsCompleted)
                return Task.CompletedTask;

            // Make sure we don't introduce weird locking issues  
            // blocking on the completion source by jumping onto
            // a new thread to wait
            return completion.Task;
        }

        protected static void ExceptionMaskedAction(Action action, bool maskExceptions)
        {
            try
            {
                action();
            }
            catch (TargetInvocationException exception)
            {
                Log.Trace("Exception throw when invoking action via dispatcher", exception);
                if (maskExceptions)
                    Log.Warning("TargetInvocateException masked " + exception.InnerException);
                else
                    throw;
            }
            catch (Exception exception)
            {
                Log.Trace("Exception throw when invoking action via dispatcher", exception);
                if (maskExceptions)
                    Log.Warning("Exception masked " + exception);
                else
                    throw;
            }
        }
    }
}
