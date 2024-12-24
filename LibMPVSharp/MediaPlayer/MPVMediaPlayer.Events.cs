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
        public delegate void PropertyChanged(ref MpvEventProperty property);
        public event PropertyChanged? MPVPropertyChanged;

        private void MPVWeakup(IntPtr ctx)
        {
            if (_eventLoopTask == null)
            {
                _eventLoopTask = Task.Factory.StartNew(() =>
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
                }, TaskCreationOptions.LongRunning);
            }
        }

        private void OnMPVEvents(double timeout)
        {
            var mpvEvent = Client.MpvWaitEvent(_clientHandle, timeout);

            switch (mpvEvent->event_id)
            {
                case MpvEventId.MPV_EVENT_NONE:
                    break;
                case MpvEventId.MPV_EVENT_SHUTDOWN:
                    break;
                case MpvEventId.MPV_EVENT_LOG_MESSAGE:
                    break;
                case MpvEventId.MPV_EVENT_GET_PROPERTY_REPLY:
                    break;
                case MpvEventId.MPV_EVENT_SET_PROPERTY_REPLY:
                    break;
                case MpvEventId.MPV_EVENT_COMMAND_REPLY:
                    break;
                case MpvEventId.MPV_EVENT_START_FILE:
                    break;
                case MpvEventId.MPV_EVENT_END_FILE:
                    break;
                case MpvEventId.MPV_EVENT_FILE_LOADED:
                    break;
                case MpvEventId.MPV_EVENT_IDLE:
                    break;
                case MpvEventId.MPV_EVENT_TICK:
                    break;
                case MpvEventId.MPV_EVENT_CLIENT_MESSAGE:
                    break;
                case MpvEventId.MPV_EVENT_VIDEO_RECONFIG:
                    break;
                case MpvEventId.MPV_EVENT_AUDIO_RECONFIG:
                    break;
                case MpvEventId.MPV_EVENT_SEEK:
                    break;
                case MpvEventId.MPV_EVENT_PLAYBACK_RESTART:
                    break;
                case MpvEventId.MPV_EVENT_PROPERTY_CHANGE:
                    var property = Marshal.PtrToStructure<MpvEventProperty>((IntPtr)mpvEvent->data);
                    MPVPropertyChanged?.Invoke(ref property);
                    break;
                case MpvEventId.MPV_EVENT_QUEUE_OVERFLOW:
                    break;
                case MpvEventId.MPV_EVENT_HOOK:
                    break;
            }
        }
    }
}
