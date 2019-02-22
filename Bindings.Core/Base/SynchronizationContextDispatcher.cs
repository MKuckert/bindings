using System;
using System.Threading;

namespace Bindings.Core.Base
{
    public class SynchronizationContextDispatcher : MainThreadAsyncDispatcher
    {
        private readonly SynchronizationContext _synchronizationContext;
        private readonly int _mainThreadId;

        public SynchronizationContextDispatcher()
        {
            _synchronizationContext = SynchronizationContext.Current;
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        protected override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (Thread.CurrentThread.ManagedThreadId == _mainThreadId)
            {
                ExceptionMaskedAction(action, maskExceptions);
            }
            else
            {
                _synchronizationContext.Post((_) => ExceptionMaskedAction(action, maskExceptions), null);
            }
            return true;
        }
    }
}