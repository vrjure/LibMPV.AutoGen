using System;

namespace LibMPVSharp.Wraps
{
    internal class DisposableObject : IDisposable
    {
        private readonly Action _disposeAction;
        public DisposableObject(Action disposeAction)
        {
            _disposeAction = disposeAction;
        }
        public void Dispose()
        {
            _disposeAction?.Invoke();
        }
    }
}
