﻿using LibMPVSharp.Wraps;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibMPVSharp
{
    public unsafe partial class MPVMediaPlayer : IDisposable
    {
        private readonly MPVMediaPlayerOptions _options;
        private MpvSetWakeupCallback_cbCallback? _wakeupCallback;
        private MpvHandle* _clientHandle;
        private bool _disposed;

        public IntPtr MPVHandle => (IntPtr)_clientHandle;
        public MPVMediaPlayerOptions Options => _options;

        static MPVMediaPlayer()
        {
            LibraryName.DllImportResolver();
        }

        public MPVMediaPlayer() : this(new MPVMediaPlayerOptions())
        {
            
        }

        public MPVMediaPlayer(Action<MPVMediaPlayer> beforeInitialize) : this(new MPVMediaPlayerOptions{ BeforeInitialize = beforeInitialize })
        {
            
        }

        public MPVMediaPlayer(MPVMediaPlayerOptions options) 
        {
            _options = options;
            Debug.WriteLine("Api version:{0}", Client.MpvClientApiVersion());
            if (_options.SharedPlayer == null)
            {
                _clientHandle = Client.MpvCreate();
                Initialize();
                return;
            }
            else if (_options.IsWeakReference)
            {
                _clientHandle = Client.MpvCreateWeakClient(_options.SharedPlayer._clientHandle, _options.SharePlayerName);
            }
            else
            {
                _clientHandle = Client.MpvCreateClient(_options.SharedPlayer._clientHandle, _options.SharePlayerName);
            }
            Initialize(false);
        }

        private void Initialize(bool initialize = true)
        {
            CheckClientHandle();

            if (initialize)
            {
                _options.BeforeInitialize?.Invoke(this);
                var error = Client.MpvInitialize(_clientHandle);
                CheckError(error, nameof(Client.MpvInitialize));
            }
            
            SetProperty(VideoOpts.Vo , "libmpv");
            _wakeupCallback = MPVWeakup;
            Client.MpvSetWakeupCallback(_clientHandle, _wakeupCallback, _clientHandle);
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

        public MpvNodeWrap GetPropertyNode(string name)
        {
            CheckClientHandle();
            var array = new MpvNode[1];
            fixed (MpvNode* node = array)
            {
                var error = Client.MpvGetProperty(_clientHandle, name, MpvFormat.MPV_FORMAT_NODE, node);
                CheckError(error, nameof(Client.MpvGetProperty), name);
                return new MpvNodeWrap(node, array[0]);
            }
        }

        public void ExecuteCommand(params string[] args)
        {
            CheckClientHandle();

            var rootPtr = GetStringArrayPointer(args, out var disposable);

            try
            {
                var err = Client.MpvCommand(_clientHandle, (char**)rootPtr);
                CheckError(err, nameof(Client.MpvCommand), args);
            }
            finally
            {
                disposable?.Dispose();
            }
        }

        public Task ExecuteCommandAsync(string[] args, CancellationToken cancellation = default)
        {
            CheckClientHandle();
            var rootPtr = GetStringArrayPointer(args, out var disposable);
            TaskCompletionSource tcs = new TaskCompletionSource();
            var handle = GCHandle.Alloc(tcs);
            var userData = (ulong)GCHandle.ToIntPtr(handle);
            cancellation.Register(() =>
            {
                Client.MpvAbortAsyncCommand(_clientHandle, userData);
                tcs.TrySetCanceled();
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            });
            try
            {
                var err = Client.MpvCommandAsync(_clientHandle, userData, (char**)rootPtr);
                CheckError(err, nameof(Client.MpvCommand), args);
            }
            catch (Exception ex)
            {
                handle.Free();
                tcs.TrySetException(ex);
            }
            finally
            {
                disposable?.Dispose();
            }
            return tcs.Task;
        }

        public MpvNodeWrap? ExecuteCommandNode(MpvNode args, ref MpvNode? result)
        {
            CheckClientHandle();
            var arr = new MpvNode[] { args };
            int err = 0;
            fixed (MpvNode* nodePtr = arr)
            {
                if (result.HasValue)
                {
                    var resultArray = new MpvNode[]{ result.Value };
                    fixed (MpvNode* resultPtr = resultArray)
                    {
                        err = Client.MpvCommandNode(_clientHandle, nodePtr, resultPtr);
                        CheckError(err, nameof(Client.MpvCommandNode), "mpv node");
                        return new MpvNodeWrap(resultPtr, result.Value);
                    }
                }
                else
                {
                    err = Client.MpvCommandNode(_clientHandle, nodePtr, null);
                    CheckError(err, nameof(Client.MpvCommandNode), "mpv node");
                    return null;
                }
            }
        }
        
        public Task<MpvNodeWrap?> ExecuteCommandNodeAsync(MpvNode args, ref MpvNode? result, CancellationToken cancellation = default)
        {
            CheckClientHandle();
            var arr = new MpvNode[] { args };
            int err = 0;
            var tcs = new TaskCompletionSource<MpvNodeWrap?>();
            var handle = GCHandle.Alloc(tcs);
            var userData = (ulong)GCHandle.ToIntPtr(handle);
            cancellation.Register(() =>
            {
                Client.MpvAbortAsyncCommand(_clientHandle, userData);
                tcs.TrySetCanceled();
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            });
            
            try
            {
                fixed (MpvNode* nodePtr = arr)
                {
                    if (result.HasValue)
                    {
                        var resultArray = new MpvNode[]{ result.Value };
                        fixed (MpvNode* resultPtr = resultArray)
                        {
                            err = Client.MpvCommandNodeAsync(_clientHandle, userData, resultPtr);
                            CheckError(err, nameof(Client.MpvCommandNode), "mpv node");
                        }
                    }
                    else
                    {
                        err = Client.MpvCommandNodeAsync(_clientHandle, userData, null);
                        CheckError(err, nameof(Client.MpvCommandNode), "mpv node");
                    }
                }
            }
            catch (Exception e)
            {
                handle.Free();
                tcs.TrySetException(e);
            }
            return tcs.Task;
        }

        public MpvNodeWrap? ExecuteCommandRet(ref MpvNode? result, params string[] args)
        {
            CheckClientHandle();

            var rootPtr = GetStringArrayPointer(args, out var disposable);

            try
            {
                if (result.HasValue)
                {
                    var array = new MpvNode[] { result.Value };
                    fixed (MpvNode* resultPtr = array)
                    {
                        var err = Client.MpvCommandRet(_clientHandle, (char**)rootPtr, resultPtr);
                        CheckError(err, nameof(Client.MpvCommand), args);
                        return new MpvNodeWrap(resultPtr, result.Value);
                    }
                }
                else
                {
                    var err = Client.MpvCommandRet(_clientHandle, (char**)rootPtr, null);
                    CheckError(err, nameof(Client.MpvCommand), args);
                    return null;
                }
            }
            finally
            {
                disposable.Dispose();
            }
        }
        
        public void ExecuteCommandString(string args)
        {
            CheckClientHandle();
            var err = Client.MpvCommandString(_clientHandle, args);
            CheckError(err, nameof(Client.MpvCommandString), args);
        }

        public void RequestLogMessage(string min_level)
        {
            CheckClientHandle();
            var error = Client.MpvRequestLogMessages(_clientHandle, min_level);
            CheckError(error, nameof(Client.MpvRequestLogMessages), min_level);
        }
        public void RequestEvent(MpvEventId eventId, int enable)
        {
            CheckClientHandle();
            var error = Client.MpvRequestEvent(_clientHandle, eventId, enable);
            CheckError(error, nameof(Client.MpvRequestEvent), eventId.ToString(), enable.ToString());
        }

        public string GetClientName()
        {
            CheckClientHandle();
            return Client.MpvClientName(_clientHandle);
        }

        public long GetClientId()
        {
            CheckClientHandle();
            return Client.MpvClientId(_clientHandle);
        }

        public void LoadConfigFile(string filename)
        {
            CheckClientHandle();
            var error = Client.MpvLoadConfigFile(_clientHandle, filename);
            CheckError(error, nameof(Client.MpvLoadConfigFile), filename);
        }

        public long GetTimeNs()
        {
            CheckClientHandle();
            return Client.MpvGetTimeNs(_clientHandle);
        }

        public long GetTimeUS()
        {
            CheckClientHandle();
            return Client.MpvGetTimeUs(_clientHandle);
        }

        public void FreeNode(MpvNodeWrap node) => Client.MpvFreeNodeContents(node.Native);

        public void Dispose() => Dispose(false);

        public void Dispose(bool terminate)
        {
            _disposed = true;

            ReleaseRenderContext();

            if (_clientHandle == null) return;
            if (terminate)
            {
                Client.MpvTerminateDestroy(_clientHandle);
                _clientHandle = null;
            }
            else
            {
                Client.MpvDestroy(_clientHandle);
                _clientHandle = null;
            }
        }

        private IntPtr GetStringArrayPointer(string[] args, out IDisposable disposable)
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

            disposable = new DisposableObject(() =>
            {
                foreach (var item in arrPtrs)
                {
                    Marshal.FreeHGlobal(item);
                }

                Marshal.FreeHGlobal(rootPtr);
            });
            return rootPtr;
        }

        private void CheckError(int errorCode, string function, params string[] args)
        {
            if (errorCode < 0)
            {
                var error = (MpvError)errorCode;
                var msg = $"{function}({string.Join(",", args)}) error:{Client.MpvErrorString(errorCode)}";
                Debug.WriteLine(msg);
                throw new LibMPVException(error, msg);
            }
        }

        private void CheckClientHandle()
        {
            if (_clientHandle == null || _disposed) throw new LibMPVException(MpvError.MPV_ERROR_UNINITIALIZED, "Client handle is null or disposed.");
        }
    }
}