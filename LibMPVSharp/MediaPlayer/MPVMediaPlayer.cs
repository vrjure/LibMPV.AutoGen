using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibMPVSharp
{
    public unsafe partial class MPVMediaPlayer : IDisposable
    {
        private readonly MPVMediaPlayerOptions _options;
        private MpvSetWakeupCallback_cbCallback _wakeupCallback;
        private MpvHandle* _clientHandle;

        public IntPtr MPVHandle => (IntPtr)_clientHandle;
        public MPVMediaPlayerOptions Options => _options;

        public MPVMediaPlayer() : this(new MPVMediaPlayerOptions())
        {

        }

        public MPVMediaPlayer(MPVMediaPlayerOptions options) 
        {
            _options = options;

            Debug.WriteLine("Api version:{0}", Client.MpvClientApiVersion());
            _clientHandle = Client.MpvCreate();
            Initialize();
        }

        public MPVMediaPlayer(MpvHandle* clientHandle, string name)
        {
            _clientHandle = Client.MpvCreateClient(clientHandle, name);
        }

        private void Initialize()
        {
            CheckClientHandle();
            //var logPath = Path.Combine(Environment.CurrentDirectory, "mpv.log");
            //Client.MpvSetOptionString(_clientHandle, "log-file", logPath);
            //Client.MpvSetOptionString(_clientHandle, "msg-level", "all=v");
            Client.MpvSetOptionString(_clientHandle, "vo", "libmpv");
            Client.MpvSetOptionString(_clientHandle, "hwdec", "auto");

            var error = Client.MpvInitialize(_clientHandle);
            CheckError(error, nameof(Client.MpvInitialize));

            _wakeupCallback = MPVWeakup;
            Client.MpvSetWakeupCallback(_clientHandle, _wakeupCallback, IntPtr.Zero);
        }


        public void Open(Uri uri)
        {
            CheckClientHandle();
            EnsureRenderContextCreated();
            ExecuteCommand("loadfile", uri.OriginalString);
        }

        public void Play()
        {
            CheckClientHandle();
            var error = Client.MpvSetPropertyString(_clientHandle, "pause", "no");
            CheckError(error, nameof(Client.MpvSetPropertyString));
        }

        public void Pause()
        {
            CheckClientHandle();
            var error = Client.MpvSetPropertyString(_clientHandle, "pause", "yes");
        }

        public unsafe bool IsPlay()
        {
            CheckClientHandle();
            var val = Client.MpvGetPropertyString(_clientHandle, "pause");
            return val == "no";
        }

        public void SetTime(double value)
        {
            CheckClientHandle();
            ExecuteCommand("seek", value.ToString(), "absolute");
        }

        private unsafe void ExecuteCommand(params string[] args)
        {
            var count = args.Length + 1;
            var arrPtrs = new IntPtr[count];

            var rootPtr = Marshal.AllocHGlobal(IntPtr.Size * count);
            for (int i = 0; i < args.Length; i++)
            {
                var buffer = Encoding.UTF8.GetBytes(args[i] + '\0');
                var ptr = Marshal.AllocHGlobal(buffer.Length);
                Marshal.Copy(buffer, 0, ptr, buffer.Length);
                arrPtrs[i] = ptr;
            }

            Marshal.Copy(arrPtrs, 0, rootPtr, count);
            try
            {
                var err = Client.MpvCommand(_clientHandle, (char**)rootPtr);
                CheckError(err, nameof(Client.MpvCommand), args);
            }
            finally
            {
                foreach (var item in arrPtrs)
                {
                    Marshal.FreeHGlobal(item);
                }
                Marshal.FreeHGlobal(rootPtr);
            }
        }

        private void MPVWeakup(IntPtr ctx)
        {
            if (ctx == IntPtr.Zero)
            {
                return;
            }
            var mpvEvent = Marshal.PtrToStructure<MpvEvent>(ctx);
            switch (mpvEvent.event_id)
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
                    break;
                case MpvEventId.MPV_EVENT_QUEUE_OVERFLOW:
                    break;
                case MpvEventId.MPV_EVENT_HOOK:
                    break;
                default:
                    break;
            }
        }

        public void Dispose()
        {
            if (_renderContext != null)
            {
                Render.MpvRenderContextFree(_renderContext);
                _renderContext = null;
            }

            if (_clientHandle != null)
            {
                Client.MpvTerminateDestroy(_clientHandle);
                _clientHandle = null;
            }

        }

        private void CheckError(int errorCode, string function, params string[] args)
        {
            if (errorCode < 0)
            {
                var error = (MpvError)errorCode;
                var msg = $"{function}({string.Join(",", args)}) error:{error}";
                Debug.WriteLine(msg);
                throw new LibMPVException(error, msg);
            }
        }

        private void CheckClientHandle()
        {
            if (_clientHandle == null) throw new LibMPVException(MpvError.MPV_ERROR_UNINITIALIZED, "Client handle is null");
        }
    }
}
