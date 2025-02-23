using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace LibMPVSharp
{
    public unsafe partial class MPVMediaPlayer : IDisposable
    {
        private readonly MPVMediaPlayerOptions _options;
        private MpvSetWakeupCallback_cbCallback? _wakeupCallback;
        private MpvHandle* _clientHandle;
        private bool _disposed;
        private Task? _eventLoopTask;

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

        private void Initialize()
        {
            CheckClientHandle();
#if DEBUG
            var logPath = System.IO.Path.Combine(Environment.CurrentDirectory, "mpv.log");
            LogFile = logPath;
            SetProperty("log-file", logPath);
            SetProperty("msg-level", "all=v");
#endif
            SetProperty("vo", "libmpv");
            SetProperty("hwdec", "auto");

            var error = Client.MpvInitialize(_clientHandle);
            CheckError(error, nameof(Client.MpvInitialize));

            ObservableProperty("pause", MpvFormat.MPV_FORMAT_FLAG);
            ObservableProperty("duration", MpvFormat.MPV_FORMAT_DOUBLE);
            ObservableProperty("time-pos", MpvFormat.MPV_FORMAT_DOUBLE);
            ObservableProperty("volume", MpvFormat.MPV_FORMAT_INT64);
            ObservableProperty("mute", MpvFormat.MPV_FORMAT_STRING);
            ObservableProperty("speed", MpvFormat.MPV_FORMAT_DOUBLE);

            _wakeupCallback = MPVWeakup;
            Client.MpvSetWakeupCallback(_clientHandle, _wakeupCallback, null);
        }


        public void Open(string uri)
        {
            EnsureRenderContextCreated();
            ExecuteCommand("loadfile", uri);
        }

        public void SetTime(double value)
        {
            ExecuteCommand("seek", value.ToString(), "absolute");
        }

        public void Stop(bool clearPlayList = false)
        {
            if (clearPlayList)
            {
                ExecuteCommand("stop");
            }
            else
            {
                ExecuteCommand("stop", "keep-playlist");
            }
        }

        public void ObservableProperty(string name, MpvFormat format)
        {
            CheckClientHandle();
            var err = Client.MpvObserveProperty(_clientHandle, 0, name, format);
            CheckError(err, nameof(Client.MpvObserveProperty), name, format.ToString());
        }

        public void SetProperty(string name, long value)
        {
            CheckClientHandle();
            var array = new long[] { value };
            fixed(long* val = array)
            {
                var error = Client.MpvSetProperty(_clientHandle, name, MpvFormat.MPV_FORMAT_INT64, val);
                CheckError(error, nameof(Client.MpvSetProperty), name, value.ToString());
            }
        }

        public long GetPropertyLong(string name)
        {
            CheckClientHandle();
            var array = new long[] { 0 };
            fixed (long* val = array)
            {
                var error = Client.MpvGetProperty(_clientHandle, name, MpvFormat.MPV_FORMAT_INT64, val);
                CheckError(error, nameof(Client.MpvGetProperty), name);
                return array[0];
            }
        }

        public void SetProperty(string name, string? value)
        {
            CheckClientHandle();
            var error = Client.MpvSetPropertyString(_clientHandle, name, value);
            CheckError(error, nameof(Client.MpvSetPropertyString), name, value);
        }

        public string? GetPropertyString(string name)
        {
            CheckClientHandle();
            var valuePtr = Client.MpvGetPropertyString(_clientHandle, name);
            try
            {
                return Utf8StringMarshaller.ConvertToManaged(valuePtr);
            }
            finally
            {
                Client.MpvFree(valuePtr);
            }
        }

        public bool SetProperty(string name, double value)
        {
            CheckClientHandle();
            var array = new double[] { value };
            fixed(double* val = array)
            {
                var error = Client.MpvSetProperty(_clientHandle, name, MpvFormat.MPV_FORMAT_DOUBLE, val);
                value = array[0];
                return error >= 0;
            }
        }

        public double GetPropertyDouble(string name)
        {
            CheckClientHandle();
            var array = new double[] { 0 };
            fixed(double* arrayPtr = array)
            {
                var error = Client.MpvGetProperty(_clientHandle, name, MpvFormat.MPV_FORMAT_DOUBLE, arrayPtr);
                CheckError(error, nameof(Client.MpvGetProperty), name);
                return array[0];
            }
        }

        public void SetProperty(string name, bool value)
        {
            CheckClientHandle();
            bool[] array = value ? [true] : [false];
            fixed(bool* val = array)
            {
                var error = Client.MpvSetProperty(_clientHandle, name, MpvFormat.MPV_FORMAT_FLAG, val);
                CheckError(error, nameof(Client.MpvSetProperty), name, value.ToString());
            }
        }

        public bool GetPropertyBoolean(string name)
        {
            CheckClientHandle();
            bool[] array = [false];
            fixed(bool* arrayptr = array)
            {
                var error = Client.MpvGetProperty(_clientHandle, name, MpvFormat.MPV_FORMAT_FLAG, arrayptr);
                CheckError(error, nameof(Client.MpvGetProperty), name);
                return array[0];
            }
        }

        public void SetPropertyNode(string name, MpvNodeList node)
        {

        }

        public void ExecuteCommand(params string[] args)
        {
            CheckClientHandle();

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

        public void Dispose()
        {
            _disposed = true;

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