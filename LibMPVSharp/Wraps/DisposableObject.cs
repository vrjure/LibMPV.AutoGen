using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
