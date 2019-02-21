using System;
using System.Threading;
using MvvmCross.Base;

namespace Bindings
{
    public class MainThreadDispatcher : MvxMainThreadAsyncDispatcher
    {
        private readonly SynchronizationContext _synchronizationContext;
        private readonly int _mainThreadId;

        public MainThreadDispatcher()
        {
            _synchronizationContext = SynchronizationContext.Current;
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public override bool IsOnMainThread => Thread.CurrentThread.ManagedThreadId == _mainThreadId;

        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
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