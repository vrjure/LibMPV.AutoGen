﻿using System;
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
        private MpvSetWakeupCallback_cbCallback _wakeupCallback;
        private MpvHandle* _clientHandle;
        private bool _disposed;
        private Task _eventLoopTask;

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
#if DEBUG
            var logPath = System.IO.Path.Combine(Environment.CurrentDirectory, "mpv.log");
            Client.MpvSetOptionString(_clientHandle, "log-file", logPath);
            Client.MpvSetOptionString(_clientHandle, "msg-level", "all=v");
#endif

            Client.MpvSetOptionString(_clientHandle, "vo", "libmpv");
            Client.MpvSetOptionString(_clientHandle, "hwdec", "auto");

            var error = Client.MpvInitialize(_clientHandle);
            CheckError(error, nameof(Client.MpvInitialize));

            Client.MpvObserveProperty(_clientHandle, 0, "pause", MpvFormat.MPV_FORMAT_FLAG);
            Client.MpvObserveProperty(_clientHandle, 0, "duration", MpvFormat.MPV_FORMAT_DOUBLE);
            Client.MpvObserveProperty(_clientHandle, 0, "time-pos", MpvFormat.MPV_FORMAT_DOUBLE);
            Client.MpvObserveProperty(_clientHandle, 0, "volume", MpvFormat.MPV_FORMAT_INT64);
            Client.MpvObserveProperty(_clientHandle, 0, "mute", MpvFormat.MPV_FORMAT_STRING);
            Client.MpvObserveProperty(_clientHandle, 0, "speed", MpvFormat.MPV_FORMAT_DOUBLE);

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

        private void SetProperty(string name, long value)
        {
            CheckClientHandle();
            var array = new long[] { value };
            fixed(long* val = array)
            {
                var error = Client.MpvSetProperty(_clientHandle, name, MpvFormat.MPV_FORMAT_INT64, val);
                CheckError(error, nameof(Client.MpvSetProperty), name, value.ToString());
            }
        }

        private long GetPropertyLong(string name)
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

        private void SetProperty(string name, string value)
        {
            CheckClientHandle();
            var error = Client.MpvSetPropertyString(_clientHandle, name, value);
            CheckError(error, nameof(Client.MpvSetPropertyString), name, value);
        }

        private string GetPropertyString(string name)
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

        private bool SetProperty(string name, double value)
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

        private double GetPropertyDouble(string name)
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

        private void SetProperty(string name, bool value)
        {
            CheckClientHandle();
            bool[] array = value ? [true] : [false];
            fixed(bool* val = array)
            {
                var error = Client.MpvSetProperty(_clientHandle, name, MpvFormat.MPV_FORMAT_FLAG, val);
                CheckError(error, nameof(Client.MpvSetProperty), name, value.ToString());
            }
        }

        private bool GetPropertyBoolean(string name)
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

        private void ExecuteCommand(params string[] args)
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
                            Trace.WriteLine(ex);
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