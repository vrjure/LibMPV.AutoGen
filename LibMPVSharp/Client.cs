using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace LibMPVSharp
{
    public unsafe partial class Client
    {
        /// <summary>
        /// <para>Return the MPV_CLIENT_API_VERSION the mpv source has been compiled with.</para>
        /// </summary>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_client_api_version", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial uint MpvClientApiVersion();
        
        /// <summary>
        /// <para>Return a string describing the error. For unknown errors, the string</para>
        /// <para>"unknown error" is returned.</para>
        /// </summary>
        /// <param name='error'>
        /// <para>error number, see enum mpv_error</para>
        /// </param>
        /// <returns>
        /// <para>A static string describing the error. The string is completely</para>
        /// <para>static, i.e. doesn't need to be deallocated, and is valid forever.</para>
        /// </returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_error_string", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial string MpvErrorString(int error);
        
        /// <summary>
        /// <para>General function to deallocate memory returned by some of the API functions.</para>
        /// <para>Call this only if it's explicitly documented as allowed. Calling this on</para>
        /// <para>mpv memory not owned by the caller will lead to undefined behavior.</para>
        /// </summary>
        /// <param name='data'>
        /// <para>A valid pointer returned by the API, or NULL.</para>
        /// </param>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_free", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void MpvFree(void* data);
        
        /// <summary>
        /// <para>Return the name of this client handle. Every client has its own unique</para>
        /// <para>name, which is mostly used for user interface purposes.</para>
        /// </summary>
        /// <returns>
        /// <para>The client name. The string is read-only and is valid until the</para>
        /// <para>mpv_handle is destroyed.</para>
        /// </returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_client_name", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial string MpvClientName(global::LibMPVSharp.MpvHandle* ctx);
        
        /// <summary>
        /// <para>Return the ID of this client handle. Every client has its own unique ID. This</para>
        /// <para>ID is never reused by the core, even if the mpv_handle at hand gets destroyed</para>
        /// <para>and new handles get allocated.</para>
        /// <para>IDs are never 0 or negative.</para>
        /// <para>Some mpv APIs (not necessarily all) accept a name in the form "@<id>" in</para>
        /// <para>addition of the proper mpv_client_name(), where "<id>" is the ID in decimal</para>
        /// <para>form (e.g. "@123"). For example, the "script-message-to" command takes the</para>
        /// <para>client name as first argument, but also accepts the client ID formatted in</para>
        /// <para>this manner.</para>
        /// </summary>
        /// <returns>The client ID.</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_client_id", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial long MpvClientId(global::LibMPVSharp.MpvHandle* ctx);
        
        /// <summary>
        /// <para>Create a new mpv instance and an associated client API handle to control</para>
        /// <para>the mpv instance. This instance is in a pre-initialized state,</para>
        /// <para>and needs to be initialized to be actually used with most other API</para>
        /// <para>functions.</para>
        /// <para>Some API functions will return MPV_ERROR_UNINITIALIZED in the uninitialized</para>
        /// <para>state. You can call mpv_set_property() (or mpv_set_property_string() and</para>
        /// <para>other variants, and before mpv 0.21.0 mpv_set_option() etc.) to set initial</para>
        /// <para>options. After this, call mpv_initialize() to start the player, and then use</para>
        /// <para>e.g. mpv_command() to start playback of a file.</para>
        /// <para>The point of separating handle creation and actual initialization is that</para>
        /// <para>you can configure things which can't be changed during runtime.</para>
        /// <para>Unlike the command line player, this will have initial settings suitable</para>
        /// <para>for embedding in applications. The following settings are different:</para>
        /// <para>- stdin/stdout/stderr and the terminal will never be accessed. This is</para>
        /// <para>equivalent to setting the --no-terminal option.</para>
        /// <para>(Technically, this also suppresses C signal handling.)</para>
        /// <para>- No config files will be loaded. This is roughly equivalent to using</para>
        /// <para>--config=no. Since libmpv 1.15, you can actually re-enable this option,</para>
        /// <para>which will make libmpv load config files during mpv_initialize(). If you</para>
        /// <para>do this, you are strongly encouraged to set the "config-dir" option too.</para>
        /// <para>(Otherwise it will load the mpv command line player's config.)</para>
        /// <para>For example:</para>
        /// <para>mpv_set_option_string(mpv, "config-dir", "/my/path"); // set config root</para>
        /// <para>mpv_set_option_string(mpv, "config", "yes"); // enable config loading</para>
        /// <para>(call mpv_initialize() _after_ this)</para>
        /// <para>- Idle mode is enabled, which means the playback core will enter idle mode</para>
        /// <para>if there are no more files to play on the internal playlist, instead of</para>
        /// <para>exiting. This is equivalent to the --idle option.</para>
        /// <para>- Disable parts of input handling.</para>
        /// <para>- Most of the different settings can be viewed with the command line player</para>
        /// <para>by running "mpv --show-profile=libmpv".</para>
        /// <para>All this assumes that API users want a mpv instance that is strictly</para>
        /// <para>isolated from the command line player's configuration, user settings, and</para>
        /// <para>so on. You can re-enable disabled features by setting the appropriate</para>
        /// <para>options.</para>
        /// <para>The mpv command line parser is not available through this API, but you can</para>
        /// <para>set individual options with mpv_set_property(). Files for playback must be</para>
        /// <para>loaded with mpv_command() or others.</para>
        /// <para>Note that you should avoid doing concurrent accesses on the uninitialized</para>
        /// <para>client handle. (Whether concurrent access is definitely allowed or not has</para>
        /// <para>yet to be decided.)</para>
        /// </summary>
        /// <returns>
        /// <para>a new mpv client API handle. Returns NULL on error. Currently, this</para>
        /// <para>can happen in the following situations:</para>
        /// <para>- out of memory</para>
        /// <para>- LC_NUMERIC is not set to "C" (see general remarks)</para>
        /// </returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_create", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial global::LibMPVSharp.MpvHandle* MpvCreate();
        
        /// <summary>
        /// <para>Initialize an uninitialized mpv instance. If the mpv instance is already</para>
        /// <para>running, an error is returned.</para>
        /// <para>This function needs to be called to make full use of the client API if the</para>
        /// <para>client API handle was created with mpv_create().</para>
        /// <para>Only the following options are required to be set _before_ mpv_initialize():</para>
        /// <para>- options which are only read at initialization time:</para>
        /// <para>- config</para>
        /// <para>- config-dir</para>
        /// <para>- input-conf</para>
        /// <para>- load-scripts</para>
        /// <para>- script</para>
        /// <para>- player-operation-mode</para>
        /// <para>- input-app-events (macOS)</para>
        /// <para>- all encoding mode options</para>
        /// </summary>
        /// <returns>error code</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_initialize", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvInitialize(global::LibMPVSharp.MpvHandle* ctx);
        
        /// <summary>
        /// <para>Disconnect and destroy the mpv_handle. ctx will be deallocated with this</para>
        /// <para>API call.</para>
        /// <para>If the last mpv_handle is detached, the core player is destroyed. In</para>
        /// <para>addition, if there are only weak mpv_handles (such as created by</para>
        /// <para>mpv_create_weak_client() or internal scripts), these mpv_handles will</para>
        /// <para>be sent MPV_EVENT_SHUTDOWN. This function may block until these clients</para>
        /// <para>have responded to the shutdown event, and the core is finally destroyed.</para>
        /// </summary>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_destroy", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void MpvDestroy(global::LibMPVSharp.MpvHandle* ctx);
        
        /// <summary>
        /// <para>Similar to mpv_destroy(), but brings the player and all clients down</para>
        /// <para>as well, and waits until all of them are destroyed. This function blocks. The</para>
        /// <para>advantage over mpv_destroy() is that while mpv_destroy() merely</para>
        /// <para>detaches the client handle from the player, this function quits the player,</para>
        /// <para>waits until all other clients are destroyed (i.e. all mpv_handles are</para>
        /// <para>detached), and also waits for the final termination of the player.</para>
        /// <para>Since mpv_destroy() is called somewhere on the way, it's not safe to</para>
        /// <para>call other functions concurrently on the same context.</para>
        /// <para>Since mpv client API version 1.29:</para>
        /// <para>The first call on any mpv_handle will block until the core is destroyed.</para>
        /// <para>This means it will wait until other mpv_handle have been destroyed. If you</para>
        /// <para>want asynchronous destruction, just run the "quit" command, and then react</para>
        /// <para>to the MPV_EVENT_SHUTDOWN event.</para>
        /// <para>If another mpv_handle already called mpv_terminate_destroy(), this call will</para>
        /// <para>not actually block. It will destroy the mpv_handle, and exit immediately,</para>
        /// <para>while other mpv_handles might still be uninitializing.</para>
        /// <para>Before mpv client API version 1.29:</para>
        /// <para>If this is called on a mpv_handle that was not created with mpv_create(),</para>
        /// <para>this function will merely send a quit command and then call</para>
        /// <para>mpv_destroy(), without waiting for the actual shutdown.</para>
        /// </summary>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_terminate_destroy", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void MpvTerminateDestroy(global::LibMPVSharp.MpvHandle* ctx);
        
        /// <summary>
        /// <para>Create a new client handle connected to the same player core as ctx. This</para>
        /// <para>context has its own event queue, its own mpv_request_event() state, its own</para>
        /// <para>mpv_request_log_messages() state, its own set of observed properties, and</para>
        /// <para>its own state for asynchronous operations. Otherwise, everything is shared.</para>
        /// <para>This handle should be destroyed with mpv_destroy() if no longer</para>
        /// <para>needed. The core will live as long as there is at least 1 handle referencing</para>
        /// <para>it. Any handle can make the core quit, which will result in every handle</para>
        /// <para>receiving MPV_EVENT_SHUTDOWN.</para>
        /// <para>This function can not be called before the main handle was initialized with</para>
        /// <para>mpv_initialize(). The new handle is always initialized, unless ctx=NULL was</para>
        /// <para>passed.</para>
        /// </summary>
        /// <param name='ctx'>
        /// <para>Used to get the reference to the mpv core; handle-specific</para>
        /// <para>settings and parameters are not used.</para>
        /// <para>If NULL, this function behaves like mpv_create() (ignores name).</para>
        /// </param>
        /// <param name='name'>
        /// <para>The client name. This will be returned by mpv_client_name(). If</para>
        /// <para>the name is already in use, or contains non-alphanumeric</para>
        /// <para>characters (other than '_'), the name is modified to fit.</para>
        /// <para>If NULL, an arbitrary name is automatically chosen.</para>
        /// </param>
        /// <returns>a new handle, or NULL on error</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_create_client", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial global::LibMPVSharp.MpvHandle* MpvCreateClient(global::LibMPVSharp.MpvHandle* ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string name);
        
        /// <summary>
        /// <para>This is the same as mpv_create_client(), but the created mpv_handle is</para>
        /// <para>treated as a weak reference. If all mpv_handles referencing a core are</para>
        /// <para>weak references, the core is automatically destroyed. (This still goes</para>
        /// <para>through normal uninit of course. Effectively, if the last non-weak mpv_handle</para>
        /// <para>is destroyed, then the weak mpv_handles receive MPV_EVENT_SHUTDOWN and are</para>
        /// <para>asked to terminate as well.)</para>
        /// <para>Note if you want to use this like refcounting: you have to be aware that</para>
        /// <para>mpv_terminate_destroy() _and_ mpv_destroy() for the last non-weak</para>
        /// <para>mpv_handle will block until all weak mpv_handles are destroyed.</para>
        /// </summary>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_create_weak_client", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial global::LibMPVSharp.MpvHandle* MpvCreateWeakClient(global::LibMPVSharp.MpvHandle* ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string name);
        
        /// <summary>
        /// <para>Load a config file. This loads and parses the file, and sets every entry in</para>
        /// <para>the config file's default section as if mpv_set_option_string() is called.</para>
        /// <para>The filename should be an absolute path. If it isn't, the actual path used</para>
        /// <para>is unspecified. (Note: an absolute path starts with '/' on UNIX.) If the</para>
        /// <para>file wasn't found, MPV_ERROR_INVALID_PARAMETER is returned.</para>
        /// <para>If a fatal error happens when parsing a config file, MPV_ERROR_OPTION_ERROR</para>
        /// <para>is returned. Errors when setting options as well as other types or errors</para>
        /// <para>are ignored (even if options do not exist). You can still try to capture</para>
        /// <para>the resulting error messages with mpv_request_log_messages(). Note that it's</para>
        /// <para>possible that some options were successfully set even if any of these errors</para>
        /// <para>happen.</para>
        /// </summary>
        /// <param name='filename'>
        /// <para>absolute path to the config file on the local filesystem</para>
        /// </param>
        /// <returns>error code</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_load_config_file", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvLoadConfigFile(global::LibMPVSharp.MpvHandle* ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string filename);
        
        /// <summary>
        /// <para>Return the internal time in nanoseconds. This has an arbitrary start offset,</para>
        /// <para>but will never wrap or go backwards.</para>
        /// <para>Note that this is always the real time, and doesn't necessarily have to do</para>
        /// <para>with playback time. For example, playback could go faster or slower due to</para>
        /// <para>playback speed, or due to playback being paused. Use the "time-pos" property</para>
        /// <para>instead to get the playback status.</para>
        /// <para>Unlike other libmpv APIs, this can be called at absolutely any time (even</para>
        /// <para>within wakeup callbacks), as long as the context is valid.</para>
        /// <para>Safe to be called from mpv render API threads.</para>
        /// </summary>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_get_time_ns", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial long MpvGetTimeNs(global::LibMPVSharp.MpvHandle* ctx);
        
        /// <summary>
        /// <para>Same as mpv_get_time_ns but in microseconds.</para>
        /// </summary>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_get_time_us", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial long MpvGetTimeUs(global::LibMPVSharp.MpvHandle* ctx);
        
        /// <summary>
        /// <para>Frees any data referenced by the node. It doesn't free the node itself.</para>
        /// <para>Call this only if the mpv client API set the node. If you constructed the</para>
        /// <para>node yourself (manually), you have to free it yourself.</para>
        /// <para>If node->format is MPV_FORMAT_NONE, this call does nothing. Likewise, if</para>
        /// <para>the client API sets a node with this format, this function doesn't need to</para>
        /// <para>be called. (This is just a clarification that there's no danger of anything</para>
        /// <para>strange happening in these cases.)</para>
        /// </summary>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_free_node_contents", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void MpvFreeNodeContents(global::LibMPVSharp.MpvNode* node);
        
        /// <summary>
        /// <para>Set an option. Note that you can't normally set options during runtime. It</para>
        /// <para>works in uninitialized state (see mpv_create()), and in some cases in at</para>
        /// <para>runtime.</para>
        /// <para>Using a format other than MPV_FORMAT_NODE is equivalent to constructing a</para>
        /// <para>mpv_node with the given format and data, and passing the mpv_node to this</para>
        /// <para>function.</para>
        /// <para>Note: this is semi-deprecated. For most purposes, this is not needed anymore.</para>
        /// <para>Starting with mpv version 0.21.0 (version 1.23) most options can be set</para>
        /// <para>with mpv_set_property() (and related functions), and even before</para>
        /// <para>mpv_initialize(). In some obscure corner cases, using this function</para>
        /// <para>to set options might still be required (see</para>
        /// <para>"Inconsistencies between options and properties" in the manpage). Once</para>
        /// <para>these are resolved, the option setting functions might be fully</para>
        /// <para>deprecated.</para>
        /// </summary>
        /// <param name='name'>
        /// <para>Option name. This is the same as on the mpv command line, but</para>
        /// <para>without the leading "--".</para>
        /// </param>
        /// <param name='format'>
        /// <para>see enum mpv_format.</para>
        /// </param>
        /// <param name='data'>
        /// <para>Option value (according to the format).</para>
        /// </param>
        /// <returns>error code</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_set_option", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvSetOption(global::LibMPVSharp.MpvHandle* ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string name, global::LibMPVSharp.MpvFormat format, void* data);
        
        /// <summary>
        /// <para>Convenience function to set an option to a string value. This is like</para>
        /// <para>calling mpv_set_option() with MPV_FORMAT_STRING.</para>
        /// </summary>
        /// <returns>error code</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_set_option_string", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvSetOptionString(global::LibMPVSharp.MpvHandle* ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string name, [MarshalAs(UnmanagedType.LPUTF8Str)]string data);
        
        /// <summary>
        /// <para>Send a command to the player. Commands are the same as those used in</para>
        /// <para>input.conf, except that this function takes parameters in a pre-split</para>
        /// <para>form.</para>
        /// <para>The commands and their parameters are documented in input.rst.</para>
        /// <para>Does not use OSD and string expansion by default (unlike mpv_command_string()</para>
        /// <para>and input.conf).</para>
        /// </summary>
        /// <param name='args'>
        /// <para>NULL-terminated list of strings. Usually, the first item</para>
        /// <para>is the command, and the following items are arguments.</para>
        /// </param>
        /// <returns>error code</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_command", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvCommand(global::LibMPVSharp.MpvHandle* ctx, char** args);
        
        /// <summary>
        /// <para>Same as mpv_command(), but allows passing structured data in any format.</para>
        /// <para>In particular, calling mpv_command() is exactly like calling</para>
        /// <para>mpv_command_node() with the format set to MPV_FORMAT_NODE_ARRAY, and</para>
        /// <para>every arg passed in order as MPV_FORMAT_STRING.</para>
        /// <para>Does not use OSD and string expansion by default.</para>
        /// <para>The args argument can have one of the following formats:</para>
        /// <para>MPV_FORMAT_NODE_ARRAY:</para>
        /// <para>Positional arguments. Each entry is an argument using an arbitrary</para>
        /// <para>format (the format must be compatible to the used command). Usually,</para>
        /// <para>the first item is the command name (as MPV_FORMAT_STRING). The order</para>
        /// <para>of arguments is as documented in each command description.</para>
        /// <para>MPV_FORMAT_NODE_MAP:</para>
        /// <para>Named arguments. This requires at least an entry with the key "name"</para>
        /// <para>to be present, which must be a string, and contains the command name.</para>
        /// <para>The special entry "_flags" is optional, and if present, must be an</para>
        /// <para>array of strings, each being a command prefix to apply. All other</para>
        /// <para>entries are interpreted as arguments. They must use the argument names</para>
        /// <para>as documented in each command description. Some commands do not</para>
        /// <para>support named arguments at all, and must use MPV_FORMAT_NODE_ARRAY.</para>
        /// </summary>
        /// <param name='args'>
        /// <para>mpv_node with format set to one of the values documented</para>
        /// <para>above (see there for details)</para>
        /// </param>
        /// <param name='result'>
        /// <para>Optional, pass NULL if unused. If not NULL, and if the</para>
        /// <para>function succeeds, this is set to command-specific return</para>
        /// <para>data. You must call mpv_free_node_contents() to free it</para>
        /// <para>(again, only if the command actually succeeds).</para>
        /// <para>Not many commands actually use this at all.</para>
        /// </param>
        /// <returns>error code (the result parameter is not set on error)</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_command_node", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvCommandNode(global::LibMPVSharp.MpvHandle* ctx, global::LibMPVSharp.MpvNode* args, global::LibMPVSharp.MpvNode* result);
        
        /// <summary>
        /// <para>This is essentially identical to mpv_command() but it also returns a result.</para>
        /// <para>Does not use OSD and string expansion by default.</para>
        /// </summary>
        /// <param name='args'>
        /// <para>NULL-terminated list of strings. Usually, the first item</para>
        /// <para>is the command, and the following items are arguments.</para>
        /// </param>
        /// <param name='result'>
        /// <para>Optional, pass NULL if unused. If not NULL, and if the</para>
        /// <para>function succeeds, this is set to command-specific return</para>
        /// <para>data. You must call mpv_free_node_contents() to free it</para>
        /// <para>(again, only if the command actually succeeds).</para>
        /// <para>Not many commands actually use this at all.</para>
        /// </param>
        /// <returns>error code (the result parameter is not set on error)</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_command_ret", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvCommandRet(global::LibMPVSharp.MpvHandle* ctx, char** args, global::LibMPVSharp.MpvNode* result);
        
        /// <summary>
        /// <para>Same as mpv_command, but use input.conf parsing for splitting arguments.</para>
        /// <para>This is slightly simpler, but also more error prone, since arguments may</para>
        /// <para>need quoting/escaping.</para>
        /// <para>This also has OSD and string expansion enabled by default.</para>
        /// </summary>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_command_string", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvCommandString(global::LibMPVSharp.MpvHandle* ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string args);
        
        /// <summary>
        /// <para>Same as mpv_command, but run the command asynchronously.</para>
        /// <para>Commands are executed asynchronously. You will receive a</para>
        /// <para>MPV_EVENT_COMMAND_REPLY event. This event will also have an</para>
        /// <para>error code set if running the command failed. For commands that</para>
        /// <para>return data, the data is put into mpv_event_command.result.</para>
        /// <para>The only case when you do not receive an event is when the function call</para>
        /// <para>itself fails. This happens only if parsing the command itself (or otherwise</para>
        /// <para>validating it) fails, i.e. the return code of the API call is not 0 or</para>
        /// <para>positive.</para>
        /// <para>Safe to be called from mpv render API threads.</para>
        /// </summary>
        /// <param name='reply_userdata'>
        /// <para>the value mpv_event.reply_userdata of the reply will</para>
        /// <para>be set to (see section about asynchronous calls)</para>
        /// </param>
        /// <param name='args'>
        /// <para>NULL-terminated list of strings (see mpv_command())</para>
        /// </param>
        /// <returns>error code (if parsing or queuing the command fails)</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_command_async", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvCommandAsync(global::LibMPVSharp.MpvHandle* ctx, ulong reply_userdata, char** args);
        
        /// <summary>
        /// <para>Same as mpv_command_node(), but run it asynchronously. Basically, this</para>
        /// <para>function is to mpv_command_node() what mpv_command_async() is to</para>
        /// <para>mpv_command().</para>
        /// <para>See mpv_command_async() for details.</para>
        /// <para>Safe to be called from mpv render API threads.</para>
        /// </summary>
        /// <param name='reply_userdata'>
        /// <para>the value mpv_event.reply_userdata of the reply will</para>
        /// <para>be set to (see section about asynchronous calls)</para>
        /// </param>
        /// <param name='args'>
        /// <para>as in mpv_command_node()</para>
        /// </param>
        /// <returns>error code (if parsing or queuing the command fails)</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_command_node_async", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvCommandNodeAsync(global::LibMPVSharp.MpvHandle* ctx, ulong reply_userdata, global::LibMPVSharp.MpvNode* args);
        
        /// <summary>
        /// <para>Signal to all async requests with the matching ID to abort. This affects</para>
        /// <para>the following API calls:</para>
        /// <para>mpv_command_async</para>
        /// <para>mpv_command_node_async</para>
        /// <para>All of these functions take a reply_userdata parameter. This API function</para>
        /// <para>tells all requests with the matching reply_userdata value to try to return</para>
        /// <para>as soon as possible. If there are multiple requests with matching ID, it</para>
        /// <para>aborts all of them.</para>
        /// <para>This API function is mostly asynchronous itself. It will not wait until the</para>
        /// <para>command is aborted. Instead, the command will terminate as usual, but with</para>
        /// <para>some work not done. How this is signaled depends on the specific command (for</para>
        /// <para>example, the "subprocess" command will indicate it by "killed_by_us" set to</para>
        /// <para>true in the result). How long it takes also depends on the situation. The</para>
        /// <para>aborting process is completely asynchronous.</para>
        /// <para>Not all commands may support this functionality. In this case, this function</para>
        /// <para>will have no effect. The same is true if the request using the passed</para>
        /// <para>reply_userdata has already terminated, has not been started yet, or was</para>
        /// <para>never in use at all.</para>
        /// <para>You have to be careful of race conditions: the time during which the abort</para>
        /// <para>request will be effective is _after_ e.g. mpv_command_async() has returned,</para>
        /// <para>and before the command has signaled completion with MPV_EVENT_COMMAND_REPLY.</para>
        /// </summary>
        /// <param name='reply_userdata'>
        /// <para>ID of the request to be aborted (see above)</para>
        /// </param>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_abort_async_command", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void MpvAbortAsyncCommand(global::LibMPVSharp.MpvHandle* ctx, ulong reply_userdata);
        
        /// <summary>
        /// <para>Set a property to a given value. Properties are essentially variables which</para>
        /// <para>can be queried or set at runtime. For example, writing to the pause property</para>
        /// <para>will actually pause or unpause playback.</para>
        /// <para>If the format doesn't match with the internal format of the property, access</para>
        /// <para>usually will fail with MPV_ERROR_PROPERTY_FORMAT. In some cases, the data</para>
        /// <para>is automatically converted and access succeeds. For example, MPV_FORMAT_INT64</para>
        /// <para>is always converted to MPV_FORMAT_DOUBLE, and access using MPV_FORMAT_STRING</para>
        /// <para>usually invokes a string parser. The same happens when calling this function</para>
        /// <para>with MPV_FORMAT_NODE: the underlying format may be converted to another</para>
        /// <para>type if possible.</para>
        /// <para>Using a format other than MPV_FORMAT_NODE is equivalent to constructing a</para>
        /// <para>mpv_node with the given format and data, and passing the mpv_node to this</para>
        /// <para>function. (Before API version 1.21, this was different.)</para>
        /// <para>Note: starting with mpv 0.21.0 (client API version 1.23), this can be used to</para>
        /// <para>set options in general. It even can be used before mpv_initialize()</para>
        /// <para>has been called. If called before mpv_initialize(), setting properties</para>
        /// <para>not backed by options will result in MPV_ERROR_PROPERTY_UNAVAILABLE.</para>
        /// <para>In some cases, properties and options still conflict. In these cases,</para>
        /// <para>mpv_set_property() accesses the options before mpv_initialize(), and</para>
        /// <para>the properties after mpv_initialize(). These conflicts will be removed</para>
        /// <para>in mpv 0.23.0. See mpv_set_option() for further remarks.</para>
        /// </summary>
        /// <param name='name'>
        /// <para>The property name. See input.rst for a list of properties.</para>
        /// </param>
        /// <param name='format'>
        /// <para>see enum mpv_format.</para>
        /// </param>
        /// <param name='data'>
        /// <para>Option value.</para>
        /// </param>
        /// <returns>error code</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_set_property", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvSetProperty(global::LibMPVSharp.MpvHandle* ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string name, global::LibMPVSharp.MpvFormat format, void* data);
        
        /// <summary>
        /// <para>Convenience function to set a property to a string value.</para>
        /// <para>This is like calling mpv_set_property() with MPV_FORMAT_STRING.</para>
        /// </summary>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_set_property_string", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvSetPropertyString(global::LibMPVSharp.MpvHandle* ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string name, [MarshalAs(UnmanagedType.LPUTF8Str)]string data);
        
        /// <summary>
        /// <para>Convenience function to delete a property.</para>
        /// <para>This is equivalent to running the command "del [name]".</para>
        /// </summary>
        /// <param name='name'>
        /// <para>The property name. See input.rst for a list of properties.</para>
        /// </param>
        /// <returns>error code</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_del_property", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvDelProperty(global::LibMPVSharp.MpvHandle* ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string name);
        
        /// <summary>
        /// <para>Set a property asynchronously. You will receive the result of the operation</para>
        /// <para>as MPV_EVENT_SET_PROPERTY_REPLY event. The mpv_event.error field will contain</para>
        /// <para>the result status of the operation. Otherwise, this function is similar to</para>
        /// <para>mpv_set_property().</para>
        /// <para>Safe to be called from mpv render API threads.</para>
        /// </summary>
        /// <param name='reply_userdata'>
        /// <para>see section about asynchronous calls</para>
        /// </param>
        /// <param name='name'>
        /// <para>The property name.</para>
        /// </param>
        /// <param name='format'>
        /// <para>see enum mpv_format.</para>
        /// </param>
        /// <param name='data'>
        /// <para>Option value. The value will be copied by the function. It</para>
        /// <para>will never be modified by the client API.</para>
        /// </param>
        /// <returns>error code if sending the request failed</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_set_property_async", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvSetPropertyAsync(global::LibMPVSharp.MpvHandle* ctx, ulong reply_userdata, [MarshalAs(UnmanagedType.LPUTF8Str)]string name, global::LibMPVSharp.MpvFormat format, void* data);
        
        /// <summary>
        /// <para>Read the value of the given property.</para>
        /// <para>If the format doesn't match with the internal format of the property, access</para>
        /// <para>usually will fail with MPV_ERROR_PROPERTY_FORMAT. In some cases, the data</para>
        /// <para>is automatically converted and access succeeds. For example, MPV_FORMAT_INT64</para>
        /// <para>is always converted to MPV_FORMAT_DOUBLE, and access using MPV_FORMAT_STRING</para>
        /// <para>usually invokes a string formatter.</para>
        /// </summary>
        /// <param name='name'>
        /// <para>The property name.</para>
        /// </param>
        /// <param name='format'>
        /// <para>see enum mpv_format.</para>
        /// </param>
        /// <param name='data'>
        /// <para>Pointer to the variable holding the option value. On</para>
        /// <para>success, the variable will be set to a copy of the option</para>
        /// <para>value. For formats that require dynamic memory allocation,</para>
        /// <para>you can free the value with mpv_free() (strings) or</para>
        /// <para>mpv_free_node_contents() (MPV_FORMAT_NODE).</para>
        /// </param>
        /// <returns>error code</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_get_property", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvGetProperty(global::LibMPVSharp.MpvHandle* ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string name, global::LibMPVSharp.MpvFormat format, void* data);
        
        /// <summary>
        /// <para>Return the value of the property with the given name as string. This is</para>
        /// <para>equivalent to mpv_get_property() with MPV_FORMAT_STRING.</para>
        /// <para>See MPV_FORMAT_STRING for character encoding issues.</para>
        /// <para>On error, NULL is returned. Use mpv_get_property() if you want fine-grained</para>
        /// <para>error reporting.</para>
        /// </summary>
        /// <param name='name'>
        /// <para>The property name.</para>
        /// </param>
        /// <returns>
        /// <para>Property value, or NULL if the property can't be retrieved. Free</para>
        /// <para>the string with mpv_free().</para>
        /// </returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_get_property_string", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial string MpvGetPropertyString(global::LibMPVSharp.MpvHandle* ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string name);
        
        /// <summary>
        /// <para>Return the property as "OSD" formatted string. This is the same as</para>
        /// <para>mpv_get_property_string, but using MPV_FORMAT_OSD_STRING.</para>
        /// </summary>
        /// <returns>
        /// <para>Property value, or NULL if the property can't be retrieved. Free</para>
        /// <para>the string with mpv_free().</para>
        /// </returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_get_property_osd_string", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial string MpvGetPropertyOsdString(global::LibMPVSharp.MpvHandle* ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string name);
        
        /// <summary>
        /// <para>Get a property asynchronously. You will receive the result of the operation</para>
        /// <para>as well as the property data with the MPV_EVENT_GET_PROPERTY_REPLY event.</para>
        /// <para>You should check the mpv_event.error field on the reply event.</para>
        /// <para>Safe to be called from mpv render API threads.</para>
        /// </summary>
        /// <param name='reply_userdata'>
        /// <para>see section about asynchronous calls</para>
        /// </param>
        /// <param name='name'>
        /// <para>The property name.</para>
        /// </param>
        /// <param name='format'>
        /// <para>see enum mpv_format.</para>
        /// </param>
        /// <returns>error code if sending the request failed</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_get_property_async", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvGetPropertyAsync(global::LibMPVSharp.MpvHandle* ctx, ulong reply_userdata, [MarshalAs(UnmanagedType.LPUTF8Str)]string name, global::LibMPVSharp.MpvFormat format);
        
        /// <summary>
        /// <para>Get a notification whenever the given property changes. You will receive</para>
        /// <para>updates as MPV_EVENT_PROPERTY_CHANGE. Note that this is not very precise:</para>
        /// <para>for some properties, it may not send updates even if the property changed.</para>
        /// <para>This depends on the property, and it's a valid feature request to ask for</para>
        /// <para>better update handling of a specific property. (For some properties, like</para>
        /// <para>``clock``, which shows the wall clock, this mechanism doesn't make too</para>
        /// <para>much sense anyway.)</para>
        /// <para>Property changes are coalesced: the change events are returned only once the</para>
        /// <para>event queue becomes empty (e.g. mpv_wait_event() would block or return</para>
        /// <para>MPV_EVENT_NONE), and then only one event per changed property is returned.</para>
        /// <para>You always get an initial change notification. This is meant to initialize</para>
        /// <para>the user's state to the current value of the property.</para>
        /// <para>Normally, change events are sent only if the property value changes according</para>
        /// <para>to the requested format. mpv_event_property will contain the property value</para>
        /// <para>as data member.</para>
        /// <para>Warning: if a property is unavailable or retrieving it caused an error,</para>
        /// <para>MPV_FORMAT_NONE will be set in mpv_event_property, even if the</para>
        /// <para>format parameter was set to a different value. In this case, the</para>
        /// <para>mpv_event_property.data field is invalid.</para>
        /// <para>If the property is observed with the format parameter set to MPV_FORMAT_NONE,</para>
        /// <para>you get low-level notifications whether the property _may_ have changed, and</para>
        /// <para>the data member in mpv_event_property will be unset. With this mode, you</para>
        /// <para>will have to determine yourself whether the property really changed. On the</para>
        /// <para>other hand, this mechanism can be faster and uses less resources.</para>
        /// <para>Observing a property that doesn't exist is allowed. (Although it may still</para>
        /// <para>cause some sporadic change events.)</para>
        /// <para>Keep in mind that you will get change notifications even if you change a</para>
        /// <para>property yourself. Try to avoid endless feedback loops, which could happen</para>
        /// <para>if you react to the change notifications triggered by your own change.</para>
        /// <para>Only the mpv_handle on which this was called will receive the property</para>
        /// <para>change events, or can unobserve them.</para>
        /// <para>Safe to be called from mpv render API threads.</para>
        /// </summary>
        /// <param name='reply_userdata'>
        /// <para>This will be used for the mpv_event.reply_userdata</para>
        /// <para>field for the received MPV_EVENT_PROPERTY_CHANGE</para>
        /// <para>events. (Also see section about asynchronous calls,</para>
        /// <para>although this function is somewhat different from</para>
        /// <para>actual asynchronous calls.)</para>
        /// <para>If you have no use for this, pass 0.</para>
        /// <para>Also see mpv_unobserve_property().</para>
        /// </param>
        /// <param name='name'>
        /// <para>The property name.</para>
        /// </param>
        /// <param name='format'>
        /// <para>see enum mpv_format. Can be MPV_FORMAT_NONE to omit values</para>
        /// <para>from the change events.</para>
        /// </param>
        /// <returns>error code (usually fails only on OOM or unsupported format)</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_observe_property", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvObserveProperty(global::LibMPVSharp.MpvHandle* mpv, ulong reply_userdata, [MarshalAs(UnmanagedType.LPUTF8Str)]string name, global::LibMPVSharp.MpvFormat format);
        
        /// <summary>
        /// <para>Undo mpv_observe_property(). This will remove all observed properties for</para>
        /// <para>which the given number was passed as reply_userdata to mpv_observe_property.</para>
        /// <para>Safe to be called from mpv render API threads.</para>
        /// </summary>
        /// <param name='registered_reply_userdata'>
        /// <para>ID that was passed to mpv_observe_property</para>
        /// </param>
        /// <returns>
        /// <para>negative value is an error code, >=0 is number of removed properties</para>
        /// <para>on success (includes the case when 0 were removed)</para>
        /// </returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_unobserve_property", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvUnobserveProperty(global::LibMPVSharp.MpvHandle* mpv, ulong registered_reply_userdata);
        
        /// <summary>
        /// <para>Return a string describing the event. For unknown events, NULL is returned.</para>
        /// <para>Note that all events actually returned by the API will also yield a non-NULL</para>
        /// <para>string with this function.</para>
        /// </summary>
        /// <param name='event'>
        /// <para>event ID, see see enum mpv_event_id</para>
        /// </param>
        /// <returns>
        /// <para>A static string giving a short symbolic name of the event. It</para>
        /// <para>consists of lower-case alphanumeric characters and can include "-"</para>
        /// <para>characters. This string is suitable for use in e.g. scripting</para>
        /// <para>interfaces.</para>
        /// <para>The string is completely static, i.e. doesn't need to be deallocated,</para>
        /// <para>and is valid forever.</para>
        /// </returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_event_name", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial string MpvEventName(global::LibMPVSharp.MpvEventId @event);
        
        /// <summary>
        /// <para>Convert the given src event to a mpv_node, and set *dst to the result. *dst</para>
        /// <para>is set to a MPV_FORMAT_NODE_MAP, with fields for corresponding mpv_event and</para>
        /// <para>mpv_event.data/mpv_event_* fields.</para>
        /// <para>The exact details are not completely documented out of laziness. A start</para>
        /// <para>is located in the "Events" section of the manpage.</para>
        /// <para>*dst may point to newly allocated memory, or pointers in mpv_event. You must</para>
        /// <para>copy the entire mpv_node if you want to reference it after mpv_event becomes</para>
        /// <para>invalid (such as making a new mpv_wait_event() call, or destroying the</para>
        /// <para>mpv_handle from which it was returned). Call mpv_free_node_contents() to free</para>
        /// <para>any memory allocations made by this API function.</para>
        /// <para>Safe to be called from mpv render API threads.</para>
        /// </summary>
        /// <param name='dst'>
        /// <para>Target. This is not read and fully overwritten. Must be released</para>
        /// <para>with mpv_free_node_contents(). Do not write to pointers returned</para>
        /// <para>by it. (On error, this may be left as an empty node.)</para>
        /// </param>
        /// <param name='src'>
        /// <para>The source event. Not modified (it's not const due to the author's</para>
        /// <para>prejudice of the C version of const).</para>
        /// </param>
        /// <returns>error code (MPV_ERROR_NOMEM only, if at all)</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_event_to_node", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvEventToNode(global::LibMPVSharp.MpvNode* dst, global::LibMPVSharp.MpvEvent* src);
        
        /// <summary>
        /// <para>Enable or disable the given event.</para>
        /// <para>Some events are enabled by default. Some events can't be disabled.</para>
        /// <para>(Informational note: currently, all events are enabled by default, except</para>
        /// <para>MPV_EVENT_TICK.)</para>
        /// <para>Safe to be called from mpv render API threads.</para>
        /// </summary>
        /// <param name='event'>
        /// <para>See enum mpv_event_id.</para>
        /// </param>
        /// <param name='enable'>
        /// <para>1 to enable receiving this event, 0 to disable it.</para>
        /// </param>
        /// <returns>error code</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_request_event", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvRequestEvent(global::LibMPVSharp.MpvHandle* ctx, global::LibMPVSharp.MpvEventId @event, int enable);
        
        /// <summary>
        /// <para>Enable or disable receiving of log messages. These are the messages the</para>
        /// <para>command line player prints to the terminal. This call sets the minimum</para>
        /// <para>required log level for a message to be received with MPV_EVENT_LOG_MESSAGE.</para>
        /// </summary>
        /// <param name='min_level'>
        /// <para>Minimal log level as string. Valid log levels:</para>
        /// <para>no fatal error warn info v debug trace</para>
        /// <para>The value "no" disables all messages. This is the default.</para>
        /// <para>An exception is the value "terminal-default", which uses the</para>
        /// <para>log level as set by the "--msg-level" option. This works</para>
        /// <para>even if the terminal is disabled. (Since API version 1.19.)</para>
        /// <para>Also see mpv_log_level.</para>
        /// </param>
        /// <returns>error code</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_request_log_messages", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvRequestLogMessages(global::LibMPVSharp.MpvHandle* ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string min_level);
        
        /// <summary>
        /// <para>Wait for the next event, or until the timeout expires, or if another thread</para>
        /// <para>makes a call to mpv_wakeup(). Passing 0 as timeout will never wait, and</para>
        /// <para>is suitable for polling.</para>
        /// <para>The internal event queue has a limited size (per client handle). If you</para>
        /// <para>don't empty the event queue quickly enough with mpv_wait_event(), it will</para>
        /// <para>overflow and silently discard further events. If this happens, making</para>
        /// <para>asynchronous requests will fail as well (with MPV_ERROR_EVENT_QUEUE_FULL).</para>
        /// <para>Only one thread is allowed to call this on the same mpv_handle at a time.</para>
        /// <para>The API won't complain if more than one thread calls this, but it will cause</para>
        /// <para>race conditions in the client when accessing the shared mpv_event struct.</para>
        /// <para>Note that most other API functions are not restricted by this, and no API</para>
        /// <para>function internally calls mpv_wait_event(). Additionally, concurrent calls</para>
        /// <para>to different mpv_handles are always safe.</para>
        /// <para>As long as the timeout is 0, this is safe to be called from mpv render API</para>
        /// <para>threads.</para>
        /// </summary>
        /// <param name='timeout'>
        /// <para>Timeout in seconds, after which the function returns even if</para>
        /// <para>no event was received. A MPV_EVENT_NONE is returned on</para>
        /// <para>timeout. A value of 0 will disable waiting. Negative values</para>
        /// <para>will wait with an infinite timeout.</para>
        /// </param>
        /// <returns>
        /// <para>A struct containing the event ID and other data. The pointer (and</para>
        /// <para>fields in the struct) stay valid until the next mpv_wait_event()</para>
        /// <para>call, or until the mpv_handle is destroyed. You must not write to</para>
        /// <para>the struct, and all memory referenced by it will be automatically</para>
        /// <para>released by the API on the next mpv_wait_event() call, or when the</para>
        /// <para>context is destroyed. The return value is never NULL.</para>
        /// </returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_wait_event", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial global::LibMPVSharp.MpvEvent* MpvWaitEvent(global::LibMPVSharp.MpvHandle* ctx, double timeout);
        
        /// <summary>
        /// <para>Interrupt the current mpv_wait_event() call. This will wake up the thread</para>
        /// <para>currently waiting in mpv_wait_event(). If no thread is waiting, the next</para>
        /// <para>mpv_wait_event() call will return immediately (this is to avoid lost</para>
        /// <para>wakeups).</para>
        /// <para>mpv_wait_event() will receive a MPV_EVENT_NONE if it's woken up due to</para>
        /// <para>this call. But note that this dummy event might be skipped if there are</para>
        /// <para>already other events queued. All what counts is that the waiting thread</para>
        /// <para>is woken up at all.</para>
        /// <para>Safe to be called from mpv render API threads.</para>
        /// </summary>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_wakeup", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void MpvWakeup(global::LibMPVSharp.MpvHandle* ctx);
        
        /// <summary>
        /// <para>Set a custom function that should be called when there are new events. Use</para>
        /// <para>this if blocking in mpv_wait_event() to wait for new events is not feasible.</para>
        /// <para>Keep in mind that the callback will be called from foreign threads. You</para>
        /// <para>must not make any assumptions of the environment, and you must return as</para>
        /// <para>soon as possible (i.e. no long blocking waits). Exiting the callback through</para>
        /// <para>any other means than a normal return is forbidden (no throwing exceptions,</para>
        /// <para>no longjmp() calls). You must not change any local thread state (such as</para>
        /// <para>the C floating point environment).</para>
        /// <para>You are not allowed to call any client API functions inside of the callback.</para>
        /// <para>In particular, you should not do any processing in the callback, but wake up</para>
        /// <para>another thread that does all the work. The callback is meant strictly for</para>
        /// <para>notification only, and is called from arbitrary core parts of the player,</para>
        /// <para>that make no considerations for reentrant API use or allowing the callee to</para>
        /// <para>spend a lot of time doing other things. Keep in mind that it's also possible</para>
        /// <para>that the callback is called from a thread while a mpv API function is called</para>
        /// <para>(i.e. it can be reentrant).</para>
        /// <para>In general, the client API expects you to call mpv_wait_event() to receive</para>
        /// <para>notifications, and the wakeup callback is merely a helper utility to make</para>
        /// <para>this easier in certain situations. Note that it's possible that there's</para>
        /// <para>only one wakeup callback invocation for multiple events. You should call</para>
        /// <para>mpv_wait_event() with no timeout until MPV_EVENT_NONE is reached, at which</para>
        /// <para>point the event queue is empty.</para>
        /// <para>If you actually want to do processing in a callback, spawn a thread that</para>
        /// <para>does nothing but call mpv_wait_event() in a loop and dispatches the result</para>
        /// <para>to a callback.</para>
        /// <para>Only one wakeup callback can be set.</para>
        /// </summary>
        /// <param name='cb'>
        /// <para>function that should be called if a wakeup is required</para>
        /// </param>
        /// <param name='d'>
        /// <para>arbitrary userdata passed to cb</para>
        /// </param>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_set_wakeup_callback", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void MpvSetWakeupCallback(global::LibMPVSharp.MpvHandle* ctx, MpvSetWakeupCallback_cbCallback cb, void* d);
        
        /// <summary>
        /// <para>Block until all asynchronous requests are done. This affects functions like</para>
        /// <para>mpv_command_async(), which return immediately and return their result as</para>
        /// <para>events.</para>
        /// <para>This is a helper, and somewhat equivalent to calling mpv_wait_event() in a</para>
        /// <para>loop until all known asynchronous requests have sent their reply as event,</para>
        /// <para>except that the event queue is not emptied.</para>
        /// <para>In case you called mpv_suspend() before, this will also forcibly reset the</para>
        /// <para>suspend counter of the given handle.</para>
        /// </summary>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_wait_async_requests", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void MpvWaitAsyncRequests(global::LibMPVSharp.MpvHandle* ctx);
        
        /// <summary>
        /// <para>A hook is like a synchronous event that blocks the player. You register</para>
        /// <para>a hook handler with this function. You will get an event, which you need</para>
        /// <para>to handle, and once things are ready, you can let the player continue with</para>
        /// <para>mpv_hook_continue().</para>
        /// <para>Currently, hooks can't be removed explicitly. But they will be implicitly</para>
        /// <para>removed if the mpv_handle it was registered with is destroyed. This also</para>
        /// <para>continues the hook if it was being handled by the destroyed mpv_handle (but</para>
        /// <para>this should be avoided, as it might mess up order of hook execution).</para>
        /// <para>Hook handlers are ordered globally by priority and order of registration.</para>
        /// <para>Handlers for the same hook with same priority are invoked in order of</para>
        /// <para>registration (the handler registered first is run first). Handlers with</para>
        /// <para>lower priority are run first (which seems backward).</para>
        /// <para>See the "Hooks" section in the manpage to see which hooks are currently</para>
        /// <para>defined.</para>
        /// <para>Some hooks might be reentrant (so you get multiple MPV_EVENT_HOOK for the</para>
        /// <para>same hook). If this can happen for a specific hook type, it will be</para>
        /// <para>explicitly documented in the manpage.</para>
        /// <para>Only the mpv_handle on which this was called will receive the hook events,</para>
        /// <para>or can "continue" them.</para>
        /// </summary>
        /// <param name='reply_userdata'>
        /// <para>This will be used for the mpv_event.reply_userdata</para>
        /// <para>field for the received MPV_EVENT_HOOK events.</para>
        /// <para>If you have no use for this, pass 0.</para>
        /// </param>
        /// <param name='name'>
        /// <para>The hook name. This should be one of the documented names. But</para>
        /// <para>if the name is unknown, the hook event will simply be never</para>
        /// <para>raised.</para>
        /// </param>
        /// <param name='priority'>
        /// <para>See remarks above. Use 0 as a neutral default.</para>
        /// </param>
        /// <returns>error code (usually fails only on OOM)</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_hook_add", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvHookAdd(global::LibMPVSharp.MpvHandle* ctx, ulong reply_userdata, [MarshalAs(UnmanagedType.LPUTF8Str)]string name, int priority);
        
        /// <summary>
        /// <para>Respond to a MPV_EVENT_HOOK event. You must call this after you have handled</para>
        /// <para>the event. There is no way to "cancel" or "stop" the hook.</para>
        /// <para>Calling this will will typically unblock the player for whatever the hook</para>
        /// <para>is responsible for (e.g. for the "on_load" hook it lets it continue</para>
        /// <para>playback).</para>
        /// <para>It is explicitly undefined behavior to call this more than once for each</para>
        /// <para>MPV_EVENT_HOOK, to pass an incorrect ID, or to call this on a mpv_handle</para>
        /// <para>different from the one that registered the handler and received the event.</para>
        /// </summary>
        /// <param name='id'>
        /// <para>This must be the value of the mpv_event_hook.id field for the</para>
        /// <para>corresponding MPV_EVENT_HOOK.</para>
        /// </param>
        /// <returns>error code</returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_hook_continue", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvHookContinue(global::LibMPVSharp.MpvHandle* ctx, ulong id);
        
        /// <summary>
        /// <para>Return a UNIX file descriptor referring to the read end of a pipe. This</para>
        /// <para>pipe can be used to wake up a poll() based processing loop. The purpose of</para>
        /// <para>this function is very similar to mpv_set_wakeup_callback(), and provides</para>
        /// <para>a primitive mechanism to handle coordinating a foreign event loop and the</para>
        /// <para>libmpv event loop. The pipe is non-blocking. It's closed when the mpv_handle</para>
        /// <para>is destroyed. This function always returns the same value (on success).</para>
        /// <para>This is in fact implemented using the same underlying code as for</para>
        /// <para>mpv_set_wakeup_callback() (though they don't conflict), and it is as if each</para>
        /// <para>callback invocation writes a single 0 byte to the pipe. When the pipe</para>
        /// <para>becomes readable, the code calling poll() (or select()) on the pipe should</para>
        /// <para>read all contents of the pipe and then call mpv_wait_event(c, 0) until</para>
        /// <para>no new events are returned. The pipe contents do not matter and can just</para>
        /// <para>be discarded. There is not necessarily one byte per readable event in the</para>
        /// <para>pipe. For example, the pipes are non-blocking, and mpv won't block if the</para>
        /// <para>pipe is full. Pipes are normally limited to 4096 bytes, so if there are</para>
        /// <para>more than 4096 events, the number of readable bytes can not equal the number</para>
        /// <para>of events queued. Also, it's possible that mpv does not write to the pipe</para>
        /// <para>once it's guaranteed that the client was already signaled. See the example</para>
        /// <para>below how to do it correctly.</para>
        /// <para>Example:</para>
        /// <para>int pipefd = mpv_get_wakeup_pipe(mpv);</para>
        /// <para>if (pipefd</para>
        /// <para><></para>
        /// <para>0)</para>
        /// <para>error();</para>
        /// <para>while (1) {</para>
        /// <para>struct pollfd pfds[1] = {</para>
        /// <para>{ .fd = pipefd, .events = POLLIN },</para>
        /// <para>};</para>
        /// <para>// Wait until there are possibly new mpv events.</para>
        /// <para>poll(pfds, 1, -1);</para>
        /// <para>if (pfds[0].revents</para>
        /// <para>&</para>
        /// <para>POLLIN) {</para>
        /// <para>// Empty the pipe. Doing this before calling mpv_wait_event()</para>
        /// <para>// ensures that no wakeups are missed. It's not so important to</para>
        /// <para>// make sure the pipe is really empty (it will just cause some</para>
        /// <para>// additional wakeups in unlikely corner cases).</para>
        /// <para>char unused[256];</para>
        /// <para>read(pipefd, unused, sizeof(unused));</para>
        /// <para>while (1) {</para>
        /// <para>mpv_event *ev = mpv_wait_event(mpv, 0);</para>
        /// <para>// If MPV_EVENT_NONE is received, the event queue is empty.</para>
        /// <para>if (ev->event_id == MPV_EVENT_NONE)</para>
        /// <para>break;</para>
        /// <para>// Process the event.</para>
        /// <para>...</para>
        /// <para>}</para>
        /// <para>}</para>
        /// <para>}</para>
        /// </summary>
        /// <deprecated>
        /// <para>this function will be removed in the future. If you need this</para>
        /// <para>functionality, use mpv_set_wakeup_callback(), create a pipe</para>
        /// <para>manually, and call write() on your pipe in the callback.</para>
        /// </deprecated>
        /// <returns>
        /// <para>A UNIX FD of the read end of the wakeup pipe, or -1 on error.</para>
        /// <para>On MS Windows/MinGW, this will always return -1.</para>
        /// </returns>
        [LibraryImport("libmpv-2", EntryPoint = "mpv_get_wakeup_pipe", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvGetWakeupPipe(global::LibMPVSharp.MpvHandle* ctx);
        
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvHandle
    {
    }
    
    /// <summary>
    /// <para>Generic data storage.</para>
    /// <para>If mpv writes this struct (e.g. via mpv_get_property()), you must not change</para>
    /// <para>the data. In some cases (mpv_get_property()), you have to free it with</para>
    /// <para>mpv_free_node_contents(). If you fill this struct yourself, you're also</para>
    /// <para>responsible for freeing it, and you must not call mpv_free_node_contents().</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvNode
    {
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct U
        {
            public string @string;
            /// <summary>
            /// <para>valid if format==MPV_FORMAT_STRING</para>
            /// </summary>
            public int flag;
            /// <summary>
            /// <para>valid if format==MPV_FORMAT_FLAG</para>
            /// </summary>
            public long int64;
            /// <summary>
            /// <para>valid if format==MPV_FORMAT_INT64</para>
            /// </summary>
            public double double_;
            /// <summary>
            /// <para>valid if format==MPV_FORMAT_DOUBLE</para>
            /// <para>valid if format==MPV_FORMAT_NODE_ARRAY</para>
            /// <para>or if format==MPV_FORMAT_NODE_MAP</para>
            /// </summary>
            public global::LibMPVSharp.MpvNodeList* list;
            /// <summary>
            /// <para>valid if format==MPV_FORMAT_BYTE_ARRAY</para>
            /// </summary>
            public global::LibMPVSharp.MpvByteArray* ba;
        }
        public global::LibMPVSharp.MpvNode.U u;
        /// <summary>
        /// <para>Type of the data stored in this struct. This value rules what members in</para>
        /// <para>the given union can be accessed. The following formats are currently</para>
        /// <para>defined to be allowed in mpv_node:</para>
        /// <para>MPV_FORMAT_STRING       (u.string)</para>
        /// <para>MPV_FORMAT_FLAG         (u.flag)</para>
        /// <para>MPV_FORMAT_INT64        (u.int64)</para>
        /// <para>MPV_FORMAT_DOUBLE       (u.double_)</para>
        /// <para>MPV_FORMAT_NODE_ARRAY   (u.list)</para>
        /// <para>MPV_FORMAT_NODE_MAP     (u.list)</para>
        /// <para>MPV_FORMAT_BYTE_ARRAY   (u.ba)</para>
        /// <para>MPV_FORMAT_NONE         (no member)</para>
        /// <para>If you encounter a value you don't know, you must not make any</para>
        /// <para>assumptions about the contents of union u.</para>
        /// </summary>
        public global::LibMPVSharp.MpvFormat format;
    }
    
    /// <summary>
    /// <para>(see mpv_node)</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvNodeList
    {
        /// <summary>
        /// <para>Number of entries. Negative values are not allowed.</para>
        /// </summary>
        public int num;
        /// <summary>
        /// <para>MPV_FORMAT_NODE_ARRAY:</para>
        /// <para>values[N] refers to value of the Nth item</para>
        /// <para>MPV_FORMAT_NODE_MAP:</para>
        /// <para>values[N] refers to value of the Nth key/value pair</para>
        /// <para>If num > 0, values[0] to values[num-1] (inclusive) are valid.</para>
        /// <para>Otherwise, this can be NULL.</para>
        /// </summary>
        public global::LibMPVSharp.MpvNode* values;
        /// <summary>
        /// <para>MPV_FORMAT_NODE_ARRAY:</para>
        /// <para>unused (typically NULL), access is not allowed</para>
        /// <para>MPV_FORMAT_NODE_MAP:</para>
        /// <para>keys[N] refers to key of the Nth key/value pair. If num > 0, keys[0] to</para>
        /// <para>keys[num-1] (inclusive) are valid. Otherwise, this can be NULL.</para>
        /// <para>The keys are in random order. The only guarantee is that keys[N] belongs</para>
        /// <para>to the value values[N]. NULL keys are not allowed.</para>
        /// </summary>
        public char** keys;
    }
    
    /// <summary>
    /// <para>(see mpv_node)</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvByteArray
    {
        /// <summary>
        /// <para>Pointer to the data. In what format the data is stored is up to whatever</para>
        /// <para>uses MPV_FORMAT_BYTE_ARRAY.</para>
        /// </summary>
        public void* data;
        /// <summary>
        /// <para>Size of the data pointed to by ptr.</para>
        /// </summary>
        public ulong size;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvEventProperty
    {
        /// <summary>
        /// <para>Name of the property.</para>
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string name;
        /// <summary>
        /// <para>Format of the data field in the same struct. See enum mpv_format.</para>
        /// <para>This is always the same format as the requested format, except when</para>
        /// <para>the property could not be retrieved (unavailable, or an error happened),</para>
        /// <para>in which case the format is MPV_FORMAT_NONE.</para>
        /// </summary>
        public global::LibMPVSharp.MpvFormat format;
        /// <summary>
        /// <para>Received property value. Depends on the format. This is like the</para>
        /// <para>pointer argument passed to mpv_get_property().</para>
        /// <para>For example, for MPV_FORMAT_STRING you get the string with:</para>
        /// <para>char *value = *(char **)(event_property->data);</para>
        /// <para>Note that this is set to NULL if retrieving the property failed (the</para>
        /// <para>format will be MPV_FORMAT_NONE).</para>
        /// </summary>
        public void* data;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvEventLogMessage
    {
        /// <summary>
        /// <para>The module prefix, identifies the sender of the message. As a special</para>
        /// <para>case, if the message buffer overflows, this will be set to the string</para>
        /// <para>"overflow" (which doesn't appear as prefix otherwise), and the text</para>
        /// <para>field will contain an informative message.</para>
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string prefix;
        /// <summary>
        /// <para>The log level as string. See mpv_request_log_messages() for possible</para>
        /// <para>values. The level "no" is never used here.</para>
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string level;
        /// <summary>
        /// <para>The log message. It consists of 1 line of text, and is terminated with</para>
        /// <para>a newline character. (Before API version 1.6, it could contain multiple</para>
        /// <para>or partial lines.)</para>
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string text;
        /// <summary>
        /// <para>The same contents as the level field, but as a numeric ID.</para>
        /// <para>Since API version 1.6.</para>
        /// </summary>
        public global::LibMPVSharp.MpvLogLevel log_level;
    }
    
    /// <summary>
    /// <para>Since API version 1.108.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvEventStartFile
    {
        /// <summary>
        /// <para>Playlist entry ID of the file being loaded now.</para>
        /// </summary>
        public long playlist_entry_id;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvEventEndFile
    {
        /// <summary>
        /// <para>Corresponds to the values in enum mpv_end_file_reason.</para>
        /// <para>Unknown values should be treated as unknown.</para>
        /// </summary>
        public global::LibMPVSharp.MpvEndFileReason reason;
        /// <summary>
        /// <para>If reason==MPV_END_FILE_REASON_ERROR, this contains a mpv error code</para>
        /// <para>(one of MPV_ERROR_...) giving an approximate reason why playback</para>
        /// <para>failed. In other cases, this field is 0 (no error).</para>
        /// <para>Since API version 1.9.</para>
        /// </summary>
        public int error;
        /// <summary>
        /// <para>Playlist entry ID of the file that was being played or attempted to be</para>
        /// <para>played. This has the same value as the playlist_entry_id field in the</para>
        /// <para>corresponding mpv_event_start_file event.</para>
        /// <para>Since API version 1.108.</para>
        /// </summary>
        public long playlist_entry_id;
        /// <summary>
        /// <para>If loading ended, because the playlist entry to be played was for example</para>
        /// <para>a playlist, and the current playlist entry is replaced with a number of</para>
        /// <para>other entries. This may happen at least with MPV_END_FILE_REASON_REDIRECT</para>
        /// <para>(other event types may use this for similar but different purposes in the</para>
        /// <para>future). In this case, playlist_insert_id will be set to the playlist</para>
        /// <para>entry ID of the first inserted entry, and playlist_insert_num_entries to</para>
        /// <para>the total number of inserted playlist entries. Note this in this specific</para>
        /// <para>case, the ID of the last inserted entry is playlist_insert_id+num-1.</para>
        /// <para>Beware that depending on circumstances, you may observe the new playlist</para>
        /// <para>entries before seeing the event (e.g. reading the "playlist" property or</para>
        /// <para>getting a property change notification before receiving the event).</para>
        /// <para>Since API version 1.108.</para>
        /// </summary>
        public long playlist_insert_id;
        /// <summary>
        /// <para>See playlist_insert_id. Only non-0 if playlist_insert_id is valid. Never</para>
        /// <para>negative.</para>
        /// <para>Since API version 1.108.</para>
        /// </summary>
        public int playlist_insert_num_entries;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvEventClientMessage
    {
        /// <summary>
        /// <para>Arbitrary arguments chosen by the sender of the message. If num_args > 0,</para>
        /// <para>you can access args[0] through args[num_args - 1] (inclusive). What</para>
        /// <para>these arguments mean is up to the sender and receiver.</para>
        /// <para>None of the valid items are NULL.</para>
        /// </summary>
        public int num_args;
        public char** args;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvEventHook
    {
        /// <summary>
        /// <para>The hook name as passed to mpv_hook_add().</para>
        /// </summary>
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string name;
        /// <summary>
        /// <para>Internal ID that must be passed to mpv_hook_continue().</para>
        /// </summary>
        public ulong id;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvEventCommand
    {
        /// <summary>
        /// <para>Result data of the command. Note that success/failure is signaled</para>
        /// <para>separately via mpv_event.error. This field is only for result data</para>
        /// <para>in case of success. Most commands leave it at MPV_FORMAT_NONE. Set</para>
        /// <para>to MPV_FORMAT_NONE on failure.</para>
        /// </summary>
        public global::LibMPVSharp.MpvNode result;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvEvent
    {
        /// <summary>
        /// <para>One of mpv_event. Keep in mind that later ABI compatible releases might</para>
        /// <para>add new event types. These should be ignored by the API user.</para>
        /// </summary>
        public global::LibMPVSharp.MpvEventId event_id;
        /// <summary>
        /// <para>This is mainly used for events that are replies to (asynchronous)</para>
        /// <para>requests. It contains a status code, which is >= 0 on success, or</para>
        /// <para><</para>
        /// <para>0</para>
        /// <para>on error (a mpv_error value). Usually, this will be set if an</para>
        /// <para>asynchronous request fails.</para>
        /// <para>Used for:</para>
        /// <para>MPV_EVENT_GET_PROPERTY_REPLY</para>
        /// <para>MPV_EVENT_SET_PROPERTY_REPLY</para>
        /// <para>MPV_EVENT_COMMAND_REPLY</para>
        /// </summary>
        public int error;
        /// <summary>
        /// <para>If the event is in reply to a request (made with this API and this</para>
        /// <para>API handle), this is set to the reply_userdata parameter of the request</para>
        /// <para>call. Otherwise, this field is 0.</para>
        /// <para>Used for:</para>
        /// <para>MPV_EVENT_GET_PROPERTY_REPLY</para>
        /// <para>MPV_EVENT_SET_PROPERTY_REPLY</para>
        /// <para>MPV_EVENT_COMMAND_REPLY</para>
        /// <para>MPV_EVENT_PROPERTY_CHANGE</para>
        /// <para>MPV_EVENT_HOOK</para>
        /// </summary>
        public ulong reply_userdata;
        /// <summary>
        /// <para>The meaning and contents of the data member depend on the event_id:</para>
        /// <para>MPV_EVENT_GET_PROPERTY_REPLY:     mpv_event_property*</para>
        /// <para>MPV_EVENT_PROPERTY_CHANGE:        mpv_event_property*</para>
        /// <para>MPV_EVENT_LOG_MESSAGE:            mpv_event_log_message*</para>
        /// <para>MPV_EVENT_CLIENT_MESSAGE:         mpv_event_client_message*</para>
        /// <para>MPV_EVENT_START_FILE:             mpv_event_start_file* (since v1.108)</para>
        /// <para>MPV_EVENT_END_FILE:               mpv_event_end_file*</para>
        /// <para>MPV_EVENT_HOOK:                   mpv_event_hook*</para>
        /// <para>MPV_EVENT_COMMAND_REPLY*          mpv_event_command*</para>
        /// <para>other: NULL</para>
        /// <para>Note: future enhancements might add new event structs for existing or new</para>
        /// <para>event types.</para>
        /// </summary>
        public void* data;
    }
    
    
    /// <summary>
    /// <para>List of error codes than can be returned by API functions. 0 and positive</para>
    /// <para>return values always mean success, negative values are always errors.</para>
    /// </summary>
    public enum MpvError
    {
        /// <summary>
        /// <para>No error happened (used to signal successful operation).</para>
        /// <para>Keep in mind that many API functions returning error codes can also</para>
        /// <para>return positive values, which also indicate success. API users can</para>
        /// <para>hardcode the fact that ">= 0" means success.</para>
        /// </summary>
        MPV_ERROR_SUCCESS = 0,
        /// <summary>
        /// <para>The event ringbuffer is full. This means the client is choked, and can't</para>
        /// <para>receive any events. This can happen when too many asynchronous requests</para>
        /// <para>have been made, but not answered. Probably never happens in practice,</para>
        /// <para>unless the mpv core is frozen for some reason, and the client keeps</para>
        /// <para>making asynchronous requests. (Bugs in the client API implementation</para>
        /// <para>could also trigger this, e.g. if events become "lost".)</para>
        /// </summary>
        MPV_ERROR_EVENT_QUEUE_FULL = -1,
        /// <summary>
        /// <para>Memory allocation failed.</para>
        /// </summary>
        MPV_ERROR_NOMEM = -2,
        /// <summary>
        /// <para>The mpv core wasn't configured and initialized yet. See the notes in</para>
        /// <para>mpv_create().</para>
        /// </summary>
        MPV_ERROR_UNINITIALIZED = -3,
        /// <summary>
        /// <para>Generic catch-all error if a parameter is set to an invalid or</para>
        /// <para>unsupported value. This is used if there is no better error code.</para>
        /// </summary>
        MPV_ERROR_INVALID_PARAMETER = -4,
        /// <summary>
        /// <para>Trying to set an option that doesn't exist.</para>
        /// </summary>
        MPV_ERROR_OPTION_NOT_FOUND = -5,
        /// <summary>
        /// <para>Trying to set an option using an unsupported MPV_FORMAT.</para>
        /// </summary>
        MPV_ERROR_OPTION_FORMAT = -6,
        /// <summary>
        /// <para>Setting the option failed. Typically this happens if the provided option</para>
        /// <para>value could not be parsed.</para>
        /// </summary>
        MPV_ERROR_OPTION_ERROR = -7,
        /// <summary>
        /// <para>The accessed property doesn't exist.</para>
        /// </summary>
        MPV_ERROR_PROPERTY_NOT_FOUND = -8,
        /// <summary>
        /// <para>Trying to set or get a property using an unsupported MPV_FORMAT.</para>
        /// </summary>
        MPV_ERROR_PROPERTY_FORMAT = -9,
        /// <summary>
        /// <para>The property exists, but is not available. This usually happens when the</para>
        /// <para>associated subsystem is not active, e.g. querying audio parameters while</para>
        /// <para>audio is disabled.</para>
        /// </summary>
        MPV_ERROR_PROPERTY_UNAVAILABLE = -10,
        /// <summary>
        /// <para>Error setting or getting a property.</para>
        /// </summary>
        MPV_ERROR_PROPERTY_ERROR = -11,
        /// <summary>
        /// <para>General error when running a command with mpv_command and similar.</para>
        /// </summary>
        MPV_ERROR_COMMAND = -12,
        /// <summary>
        /// <para>Generic error on loading (usually used with mpv_event_end_file.error).</para>
        /// </summary>
        MPV_ERROR_LOADING_FAILED = -13,
        /// <summary>
        /// <para>Initializing the audio output failed.</para>
        /// </summary>
        MPV_ERROR_AO_INIT_FAILED = -14,
        /// <summary>
        /// <para>Initializing the video output failed.</para>
        /// </summary>
        MPV_ERROR_VO_INIT_FAILED = -15,
        /// <summary>
        /// <para>There was no audio or video data to play. This also happens if the</para>
        /// <para>file was recognized, but did not contain any audio or video streams,</para>
        /// <para>or no streams were selected.</para>
        /// </summary>
        MPV_ERROR_NOTHING_TO_PLAY = -16,
        /// <summary>
        /// <para>When trying to load the file, the file format could not be determined,</para>
        /// <para>or the file was too broken to open it.</para>
        /// </summary>
        MPV_ERROR_UNKNOWN_FORMAT = -17,
        /// <summary>
        /// <para>Generic error for signaling that certain system requirements are not</para>
        /// <para>fulfilled.</para>
        /// </summary>
        MPV_ERROR_UNSUPPORTED = -18,
        /// <summary>
        /// <para>The API function which was called is a stub only.</para>
        /// </summary>
        MPV_ERROR_NOT_IMPLEMENTED = -19,
        /// <summary>
        /// <para>Unspecified error.</para>
        /// </summary>
        MPV_ERROR_GENERIC = -20
    }
    
    /// <summary>
    /// <para>Data format for options and properties. The API functions to get/set</para>
    /// <para>properties and options support multiple formats, and this enum describes</para>
    /// <para>them.</para>
    /// </summary>
    public enum MpvFormat
    {
        /// <summary>
        /// <para>Invalid. Sometimes used for empty values. This is always defined to 0,</para>
        /// <para>so a normal 0-init of mpv_format (or e.g. mpv_node) is guaranteed to set</para>
        /// <para>this it to MPV_FORMAT_NONE (which makes some things saner as consequence).</para>
        /// </summary>
        MPV_FORMAT_NONE = 0,
        /// <summary>
        /// <para>The basic type is char*. It returns the raw property string, like</para>
        /// <para>using ${=property} in input.conf (see input.rst).</para>
        /// <para>NULL isn't an allowed value.</para>
        /// <para>Warning: although the encoding is usually UTF-8, this is not always the</para>
        /// <para>case. File tags often store strings in some legacy codepage,</para>
        /// <para>and even filenames don't necessarily have to be in UTF-8 (at</para>
        /// <para>least on Linux). If you pass the strings to code that requires</para>
        /// <para>valid UTF-8, you have to sanitize it in some way.</para>
        /// <para>On Windows, filenames are always UTF-8, and libmpv converts</para>
        /// <para>between UTF-8 and UTF-16 when using win32 API functions. See</para>
        /// <para>the "Encoding of filenames" section for details.</para>
        /// <para>Example for reading:</para>
        /// <para>char *result = NULL;</para>
        /// <para>if (mpv_get_property(ctx, "property", MPV_FORMAT_STRING,</para>
        /// <para>&result</para>
        /// <para>)</para>
        /// <para><</para>
        /// <para>0)</para>
        /// <para>goto error;</para>
        /// <para>printf("%s\n", result);</para>
        /// <para>mpv_free(result);</para>
        /// <para>Or just use mpv_get_property_string().</para>
        /// <para>Example for writing:</para>
        /// <para>char *value = "the new value";</para>
        /// <para>// yep, you pass the address to the variable</para>
        /// <para>// (needed for symmetry with other types and mpv_get_property)</para>
        /// <para>mpv_set_property(ctx, "property", MPV_FORMAT_STRING,</para>
        /// <para>&value</para>
        /// <para>);</para>
        /// <para>Or just use mpv_set_property_string().</para>
        /// </summary>
        MPV_FORMAT_STRING = 1,
        /// <summary>
        /// <para>The basic type is char*. It returns the OSD property string, like</para>
        /// <para>using ${property} in input.conf (see input.rst). In many cases, this</para>
        /// <para>is the same as the raw string, but in other cases it's formatted for</para>
        /// <para>display on OSD. It's intended to be human readable. Do not attempt to</para>
        /// <para>parse these strings.</para>
        /// <para>Only valid when doing read access. The rest works like MPV_FORMAT_STRING.</para>
        /// </summary>
        MPV_FORMAT_OSD_STRING = 2,
        /// <summary>
        /// <para>The basic type is int. The only allowed values are 0 ("no")</para>
        /// <para>and 1 ("yes").</para>
        /// <para>Example for reading:</para>
        /// <para>int result;</para>
        /// <para>if (mpv_get_property(ctx, "property", MPV_FORMAT_FLAG,</para>
        /// <para>&result</para>
        /// <para>)</para>
        /// <para><</para>
        /// <para>0)</para>
        /// <para>goto error;</para>
        /// <para>printf("%s\n", result ? "true" : "false");</para>
        /// <para>Example for writing:</para>
        /// <para>int flag = 1;</para>
        /// <para>mpv_set_property(ctx, "property", MPV_FORMAT_FLAG,</para>
        /// <para>&flag</para>
        /// <para>);</para>
        /// </summary>
        MPV_FORMAT_FLAG = 3,
        /// <summary>
        /// <para>The basic type is int64_t.</para>
        /// </summary>
        MPV_FORMAT_INT64 = 4,
        /// <summary>
        /// <para>The basic type is double.</para>
        /// </summary>
        MPV_FORMAT_DOUBLE = 5,
        /// <summary>
        /// <para>The type is mpv_node.</para>
        /// <para>For reading, you usually would pass a pointer to a stack-allocated</para>
        /// <para>mpv_node value to mpv, and when you're done you call</para>
        /// <para>mpv_free_node_contents(</para>
        /// <para>&node</para>
        /// <para>).</para>
        /// <para>You're expected not to write to the data - if you have to, copy it</para>
        /// <para>first (which you have to do manually).</para>
        /// <para>For writing, you construct your own mpv_node, and pass a pointer to the</para>
        /// <para>API. The API will never write to your data (and copy it if needed), so</para>
        /// <para>you're free to use any form of allocation or memory management you like.</para>
        /// <para>Warning: when reading, always check the mpv_node.format member. For</para>
        /// <para>example, properties might change their type in future versions</para>
        /// <para>of mpv, or sometimes even during runtime.</para>
        /// <para>Example for reading:</para>
        /// <para>mpv_node result;</para>
        /// <para>if (mpv_get_property(ctx, "property", MPV_FORMAT_NODE,</para>
        /// <para>&result</para>
        /// <para>)</para>
        /// <para><</para>
        /// <para>0)</para>
        /// <para>goto error;</para>
        /// <para>printf("format=%d\n", (int)result.format);</para>
        /// <para>mpv_free_node_contents(</para>
        /// <para>&result</para>
        /// <para>).</para>
        /// <para>Example for writing:</para>
        /// <para>mpv_node value;</para>
        /// <para>value.format = MPV_FORMAT_STRING;</para>
        /// <para>value.u.string = "hello";</para>
        /// <para>mpv_set_property(ctx, "property", MPV_FORMAT_NODE,</para>
        /// <para>&value</para>
        /// <para>);</para>
        /// </summary>
        MPV_FORMAT_NODE = 6,
        /// <summary>
        /// <para>Used with mpv_node only. Can usually not be used directly.</para>
        /// </summary>
        MPV_FORMAT_NODE_ARRAY = 7,
        /// <summary>
        /// <para>See MPV_FORMAT_NODE_ARRAY.</para>
        /// </summary>
        MPV_FORMAT_NODE_MAP = 8,
        /// <summary>
        /// <para>A raw, untyped byte array. Only used only with mpv_node, and only in</para>
        /// <para>some very specific situations. (Some commands use it.)</para>
        /// </summary>
        MPV_FORMAT_BYTE_ARRAY = 9
    }
    
    public enum MpvEventId
    {
        /// <summary>
        /// <para>Nothing happened. Happens on timeouts or sporadic wakeups.</para>
        /// </summary>
        MPV_EVENT_NONE = 0,
        /// <summary>
        /// <para>Happens when the player quits. The player enters a state where it tries</para>
        /// <para>to disconnect all clients. Most requests to the player will fail, and</para>
        /// <para>the client should react to this and quit with mpv_destroy() as soon as</para>
        /// <para>possible.</para>
        /// </summary>
        MPV_EVENT_SHUTDOWN = 1,
        /// <summary>
        /// <para>See mpv_request_log_messages().</para>
        /// </summary>
        MPV_EVENT_LOG_MESSAGE = 2,
        /// <summary>
        /// <para>Reply to a mpv_get_property_async() request.</para>
        /// <para>See also mpv_event and mpv_event_property.</para>
        /// </summary>
        MPV_EVENT_GET_PROPERTY_REPLY = 3,
        /// <summary>
        /// <para>Reply to a mpv_set_property_async() request.</para>
        /// <para>(Unlike MPV_EVENT_GET_PROPERTY, mpv_event_property is not used.)</para>
        /// </summary>
        MPV_EVENT_SET_PROPERTY_REPLY = 4,
        /// <summary>
        /// <para>Reply to a mpv_command_async() or mpv_command_node_async() request.</para>
        /// <para>See also mpv_event and mpv_event_command.</para>
        /// </summary>
        MPV_EVENT_COMMAND_REPLY = 5,
        /// <summary>
        /// <para>Notification before playback start of a file (before the file is loaded).</para>
        /// <para>See also mpv_event and mpv_event_start_file.</para>
        /// </summary>
        MPV_EVENT_START_FILE = 6,
        /// <summary>
        /// <para>Notification after playback end (after the file was unloaded).</para>
        /// <para>See also mpv_event and mpv_event_end_file.</para>
        /// </summary>
        MPV_EVENT_END_FILE = 7,
        /// <summary>
        /// <para>Notification when the file has been loaded (headers were read etc.), and</para>
        /// <para>decoding starts.</para>
        /// </summary>
        MPV_EVENT_FILE_LOADED = 8,
        /// <summary>
        /// <para>Idle mode was entered. In this mode, no file is played, and the playback</para>
        /// <para>core waits for new commands. (The command line player normally quits</para>
        /// <para>instead of entering idle mode, unless --idle was specified. If mpv</para>
        /// <para>was started with mpv_create(), idle mode is enabled by default.)</para>
        /// </summary>
        /// <deprecated>
        /// <para>This is equivalent to using mpv_observe_property() on the</para>
        /// <para>"idle-active" property. The event is redundant, and might be</para>
        /// <para>removed in the far future. As a further warning, this event</para>
        /// <para>is not necessarily sent at the right point anymore (at the</para>
        /// <para>start of the program), while the property behaves correctly.</para>
        /// </deprecated>
        MPV_EVENT_IDLE = 11,
        /// <summary>
        /// <para>Sent every time after a video frame is displayed. Note that currently,</para>
        /// <para>this will be sent in lower frequency if there is no video, or playback</para>
        /// <para>is paused - but that will be removed in the future, and it will be</para>
        /// <para>restricted to video frames only.</para>
        /// </summary>
        /// <deprecated>
        /// <para>Use mpv_observe_property() with relevant properties instead</para>
        /// <para>(such as "playback-time").</para>
        /// </deprecated>
        MPV_EVENT_TICK = 14,
        /// <summary>
        /// <para>Triggered by the script-message input command. The command uses the</para>
        /// <para>first argument of the command as client name (see mpv_client_name()) to</para>
        /// <para>dispatch the message, and passes along all arguments starting from the</para>
        /// <para>second argument as strings.</para>
        /// <para>See also mpv_event and mpv_event_client_message.</para>
        /// </summary>
        MPV_EVENT_CLIENT_MESSAGE = 16,
        /// <summary>
        /// <para>Happens after video changed in some way. This can happen on resolution</para>
        /// <para>changes, pixel format changes, or video filter changes. The event is</para>
        /// <para>sent after the video filters and the VO are reconfigured. Applications</para>
        /// <para>embedding a mpv window should listen to this event in order to resize</para>
        /// <para>the window if needed.</para>
        /// <para>Note that this event can happen sporadically, and you should check</para>
        /// <para>yourself whether the video parameters really changed before doing</para>
        /// <para>something expensive.</para>
        /// </summary>
        MPV_EVENT_VIDEO_RECONFIG = 17,
        /// <summary>
        /// <para>Similar to MPV_EVENT_VIDEO_RECONFIG. This is relatively uninteresting,</para>
        /// <para>because there is no such thing as audio output embedding.</para>
        /// </summary>
        MPV_EVENT_AUDIO_RECONFIG = 18,
        /// <summary>
        /// <para>Happens when a seek was initiated. Playback stops. Usually it will</para>
        /// <para>resume with MPV_EVENT_PLAYBACK_RESTART as soon as the seek is finished.</para>
        /// </summary>
        MPV_EVENT_SEEK = 20,
        /// <summary>
        /// <para>There was a discontinuity of some sort (like a seek), and playback</para>
        /// <para>was reinitialized. Usually happens on start of playback and after</para>
        /// <para>seeking. The main purpose is allowing the client to detect when a seek</para>
        /// <para>request is finished.</para>
        /// </summary>
        MPV_EVENT_PLAYBACK_RESTART = 21,
        /// <summary>
        /// <para>Event sent due to mpv_observe_property().</para>
        /// <para>See also mpv_event and mpv_event_property.</para>
        /// </summary>
        MPV_EVENT_PROPERTY_CHANGE = 22,
        /// <summary>
        /// <para>Happens if the internal per-mpv_handle ringbuffer overflows, and at</para>
        /// <para>least 1 event had to be dropped. This can happen if the client doesn't</para>
        /// <para>read the event queue quickly enough with mpv_wait_event(), or if the</para>
        /// <para>client makes a very large number of asynchronous calls at once.</para>
        /// <para>Event delivery will continue normally once this event was returned</para>
        /// <para>(this forces the client to empty the queue completely).</para>
        /// </summary>
        MPV_EVENT_QUEUE_OVERFLOW = 24,
        /// <summary>
        /// <para>Triggered if a hook handler was registered with mpv_hook_add(), and the</para>
        /// <para>hook is invoked. If you receive this, you must handle it, and continue</para>
        /// <para>the hook with mpv_hook_continue().</para>
        /// <para>See also mpv_event and mpv_event_hook.</para>
        /// </summary>
        MPV_EVENT_HOOK = 25
    }
    
    /// <summary>
    /// <para>Numeric log levels. The lower the number, the more important the message is.</para>
    /// <para>MPV_LOG_LEVEL_NONE is never used when receiving messages. The string in</para>
    /// <para>the comment after the value is the name of the log level as used for the</para>
    /// <para>mpv_request_log_messages() function.</para>
    /// <para>Unused numeric values are unused, but reserved for future use.</para>
    /// </summary>
    public enum MpvLogLevel
    {
        MPV_LOG_LEVEL_NONE = 0,
        /// <summary>
        /// <para>"no"    - disable absolutely all messages</para>
        /// </summary>
        MPV_LOG_LEVEL_FATAL = 10,
        /// <summary>
        /// <para>"fatal" - critical/aborting errors</para>
        /// </summary>
        MPV_LOG_LEVEL_ERROR = 20,
        /// <summary>
        /// <para>"error" - simple errors</para>
        /// </summary>
        MPV_LOG_LEVEL_WARN = 30,
        /// <summary>
        /// <para>"warn"  - possible problems</para>
        /// </summary>
        MPV_LOG_LEVEL_INFO = 40,
        /// <summary>
        /// <para>"info"  - informational message</para>
        /// </summary>
        MPV_LOG_LEVEL_V = 50,
        /// <summary>
        /// <para>"v"     - noisy informational message</para>
        /// </summary>
        MPV_LOG_LEVEL_DEBUG = 60,
        /// <summary>
        /// <para>"debug" - very noisy technical information</para>
        /// </summary>
        MPV_LOG_LEVEL_TRACE = 70
    }
    
    /// <summary>
    /// <para>Since API version 1.9.</para>
    /// </summary>
    public enum MpvEndFileReason
    {
        /// <summary>
        /// <para>The end of file was reached. Sometimes this may also happen on</para>
        /// <para>incomplete or corrupted files, or if the network connection was</para>
        /// <para>interrupted when playing a remote file. It also happens if the</para>
        /// <para>playback range was restricted with --end or --frames or similar.</para>
        /// </summary>
        MPV_END_FILE_REASON_EOF = 0,
        /// <summary>
        /// <para>Playback was stopped by an external action (e.g. playlist controls).</para>
        /// </summary>
        MPV_END_FILE_REASON_STOP = 2,
        /// <summary>
        /// <para>Playback was stopped by the quit command or player shutdown.</para>
        /// </summary>
        MPV_END_FILE_REASON_QUIT = 3,
        /// <summary>
        /// <para>Some kind of error happened that lead to playback abort. Does not</para>
        /// <para>necessarily happen on incomplete or broken files (in these cases, both</para>
        /// <para>MPV_END_FILE_REASON_ERROR or MPV_END_FILE_REASON_EOF are possible).</para>
        /// <para>mpv_event_end_file.error will be set.</para>
        /// </summary>
        MPV_END_FILE_REASON_ERROR = 4,
        /// <summary>
        /// <para>The file was a playlist or similar. When the playlist is read, its</para>
        /// <para>entries will be appended to the playlist after the entry of the current</para>
        /// <para>file, the entry of the current file is removed, and a MPV_EVENT_END_FILE</para>
        /// <para>event is sent with reason set to MPV_END_FILE_REASON_REDIRECT. Then</para>
        /// <para>playback continues with the playlist contents.</para>
        /// <para>Since API version 1.18.</para>
        /// </summary>
        MPV_END_FILE_REASON_REDIRECT = 5
    }
    
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void MpvSetWakeupCallback_cbCallback(IntPtr arg);
}
