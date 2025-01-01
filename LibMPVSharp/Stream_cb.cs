using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace LibMPVSharp
{
    public unsafe partial class Stream_cb
    {
        /// <summary>
        /// <para>Add a custom stream protocol. This will register a protocol handler under</para>
        /// <para>the given protocol prefix, and invoke the given callbacks if an URI with the</para>
        /// <para>matching protocol prefix is opened.</para>
        /// <para>The "ro" is for read-only - only read-only streams can be registered with</para>
        /// <para>this function.</para>
        /// <para>The callback remains registered until the mpv core is registered.</para>
        /// <para>If a custom stream with the same name is already registered, then the</para>
        /// <para>MPV_ERROR_INVALID_PARAMETER error is returned.</para>
        /// </summary>
        /// <param name='protocol'>
        /// <para>protocol prefix, for example "foo" for "foo://" URIs</para>
        /// </param>
        /// <param name='user_data'>
        /// <para>opaque pointer passed into the mpv_stream_cb_open_fn</para>
        /// <para>callback.</para>
        /// </param>
        /// <returns>error code</returns>
        [LibraryImport(LibraryName.Name, EntryPoint = "mpv_stream_cb_add_ro", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvStreamCbAddRo(global::LibMPVSharp.MpvHandle* ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string protocol, void* user_data, global::LibMPVSharp.MpvStreamCbOpenRoFn open_fn);
        
    }
    
    /// <summary>
    /// <para>See mpv_stream_cb_open_ro_fn callback.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvStreamCbInfo
    {
        /// <summary>
        /// <para>Opaque user-provided value, which will be passed to the other callbacks.</para>
        /// <para>The close callback will be called to release the cookie. It is not</para>
        /// <para>interpreted by mpv. It doesn't even need to be a valid pointer.</para>
        /// <para>The user sets this in the mpv_stream_cb_open_ro_fn callback.</para>
        /// </summary>
        public void* cookie;
        /// <summary>
        /// <para>Callbacks set by the user in the mpv_stream_cb_open_ro_fn callback. Some</para>
        /// <para>of them are optional, and can be left unset.</para>
        /// <para>The following callbacks are mandatory: read_fn, close_fn</para>
        /// </summary>
        public global::LibMPVSharp.MpvStreamCbReadFn read_fn;
        public global::LibMPVSharp.MpvStreamCbSeekFn seek_fn;
        public global::LibMPVSharp.MpvStreamCbSizeFn size_fn;
        public global::LibMPVSharp.MpvStreamCbCloseFn close_fn;
        public global::LibMPVSharp.MpvStreamCbCancelFn cancel_fn;
    }
    
    
    
    /// <summary>
    /// <para>Read callback used to implement a custom stream. The semantics of the</para>
    /// <para>callback match read(2) in blocking mode. Short reads are allowed (you can</para>
    /// <para>return less bytes than requested, and libmpv will retry reading the rest</para>
    /// <para>with another call). If no data can be immediately read, the callback must</para>
    /// <para>block until there is new data. A return of 0 will be interpreted as final</para>
    /// <para>EOF, although libmpv might retry the read, or seek to a different position.</para>
    /// </summary>
    /// <param name='cookie'>
    /// <para>opaque cookie identifying the stream,</para>
    /// <para>returned from mpv_stream_cb_open_fn</para>
    /// </param>
    /// <param name='buf'>
    /// <para>buffer to read data into</para>
    /// </param>
    /// <param name='size'>
    /// <para>of the buffer</para>
    /// </param>
    /// <returns>
    /// <para>number of bytes read into the buffer</para>
    /// </returns>
    /// <returns>
    /// <para>0 on EOF</para>
    /// </returns>
    /// <returns>-1 on error</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate long MpvStreamCbReadFn(void* cookie, string buf, ulong nbytes);
    
    /// <summary>
    /// <para>Seek callback used to implement a custom stream.</para>
    /// <para>Note that mpv will issue a seek to position 0 immediately after opening. This</para>
    /// <para>is used to test whether the stream is seekable (since seekability might</para>
    /// <para>depend on the URI contents, not just the protocol). Return</para>
    /// <para>MPV_ERROR_UNSUPPORTED if seeking is not implemented for this stream. This</para>
    /// <para>seek also serves to establish the fact that streams start at position 0.</para>
    /// <para>This callback can be NULL, in which it behaves as if always returning</para>
    /// <para>MPV_ERROR_UNSUPPORTED.</para>
    /// </summary>
    /// <param name='cookie'>
    /// <para>opaque cookie identifying the stream,</para>
    /// <para>returned from mpv_stream_cb_open_fn</para>
    /// </param>
    /// <param name='offset'>
    /// <para>target absolute stream position</para>
    /// </param>
    /// <returns>
    /// <para>the resulting offset of the stream</para>
    /// <para>MPV_ERROR_UNSUPPORTED or MPV_ERROR_GENERIC if the seek failed</para>
    /// </returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate long MpvStreamCbSeekFn(void* cookie, long offset);
    
    /// <summary>
    /// <para>Size callback used to implement a custom stream.</para>
    /// <para>Return MPV_ERROR_UNSUPPORTED if no size is known.</para>
    /// <para>This callback can be NULL, in which it behaves as if always returning</para>
    /// <para>MPV_ERROR_UNSUPPORTED.</para>
    /// </summary>
    /// <param name='cookie'>
    /// <para>opaque cookie identifying the stream,</para>
    /// <para>returned from mpv_stream_cb_open_fn</para>
    /// </param>
    /// <returns>the total size in bytes of the stream</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate long MpvStreamCbSizeFn(void* cookie);
    
    /// <summary>
    /// <para>Close callback used to implement a custom stream.</para>
    /// </summary>
    /// <param name='cookie'>
    /// <para>opaque cookie identifying the stream,</para>
    /// <para>returned from mpv_stream_cb_open_fn</para>
    /// </param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void MpvStreamCbCloseFn(void* cookie);
    
    /// <summary>
    /// <para>Cancel callback used to implement a custom stream.</para>
    /// <para>This callback is used to interrupt any current or future read and seek</para>
    /// <para>operations. It will be called from a separate thread than the demux</para>
    /// <para>thread, and should not block.</para>
    /// <para>This callback can be NULL.</para>
    /// <para>Available since API 1.106.</para>
    /// </summary>
    /// <param name='cookie'>
    /// <para>opaque cookie identifying the stream,</para>
    /// <para>returned from mpv_stream_cb_open_fn</para>
    /// </param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void MpvStreamCbCancelFn(void* cookie);
    
    /// <summary>
    /// <para>Open callback used to implement a custom read-only (ro) stream. The user</para>
    /// <para>must set the callback fields in the passed info struct. The cookie field</para>
    /// <para>also can be set to store state associated to the stream instance.</para>
    /// <para>Note that the info struct is valid only for the duration of this callback.</para>
    /// <para>You can't change the callbacks or the pointer to the cookie at a later point.</para>
    /// <para>Each stream instance created by the open callback can have different</para>
    /// <para>callbacks.</para>
    /// <para>The close_fn callback will terminate the stream instance. The pointers to</para>
    /// <para>your callbacks and cookie will be discarded, and the callbacks will not be</para>
    /// <para>called again.</para>
    /// </summary>
    /// <param name='user_data'>
    /// <para>opaque user data provided via mpv_stream_cb_add()</para>
    /// </param>
    /// <param name='uri'>
    /// <para>name of the stream to be opened (with protocol prefix)</para>
    /// </param>
    /// <param name='info'>
    /// <para>fields which the user should fill</para>
    /// </param>
    /// <returns>0 on success, MPV_ERROR_LOADING_FAILED if the URI cannot be opened.</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate int MpvStreamCbOpenRoFn(void* user_data, string uri, global::LibMPVSharp.MpvStreamCbInfo* info);
    
}
