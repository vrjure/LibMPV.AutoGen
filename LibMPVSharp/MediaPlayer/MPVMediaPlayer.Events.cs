using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace LibMPVSharp
{
    public unsafe partial class MPVMediaPlayer
    {
        private Task? _eventLoopTask;
        public event EventHandler<MpvEvent>? MpvEvent;

        private void MPVWeakup(IntPtr ctx)
        {
            if (_eventLoopTask == null)
            {
                _eventLoopTask = Task.Run(() =>
                {
                    while (!_disposed)
                    {
                        try
                        {
                            OnMPVEvents(-1);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    }
                });
            }
        }

        private void OnMPVEvents(double timeout)
        {
            var mpvEvent = Client.MpvWaitEvent(_clientHandle, timeout);
            MpvEvent?.Invoke(this, *mpvEvent);
            if (mpvEvent != null && mpvEvent->event_id == MpvEventId.MPV_EVENT_SHUTDOWN)
            {
                Dispose();
            }
        }
    }
}
