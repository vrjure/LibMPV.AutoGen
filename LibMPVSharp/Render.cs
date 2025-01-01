using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace LibMPVSharp
{
    public unsafe partial class Render
    {
        /// <summary>
        /// <para>Initialize the renderer state. Depending on the backend used, this will</para>
        /// <para>access the underlying GPU API and initialize its own objects.</para>
        /// <para>You must free the context with mpv_render_context_free(). Not doing so before</para>
        /// <para>the mpv core is destroyed may result in memory leaks or crashes.</para>
        /// <para>Currently, only at most 1 context can exists per mpv core (it represents the</para>
        /// <para>main video output).</para>
        /// <para>You should pass the following parameters:</para>
        /// <para>- MPV_RENDER_PARAM_API_TYPE to select the underlying backend/GPU API.</para>
        /// <para>- Backend-specific init parameter, like MPV_RENDER_PARAM_OPENGL_INIT_PARAMS.</para>
        /// <para>- Setting MPV_RENDER_PARAM_ADVANCED_CONTROL and following its rules is</para>
        /// <para>strongly recommended.</para>
        /// <para>- If you want to use hwdec, possibly hwdec interop resources.</para>
        /// </summary>
        /// <param name='res'>
        /// <para>set to the context (on success) or NULL (on failure). The value</para>
        /// <para>is never read and always overwritten.</para>
        /// </param>
        /// <param name='mpv'>
        /// <para>handle used to get the core (the mpv_render_context won't depend</para>
        /// <para>on this specific handle, only the core referenced by it)</para>
        /// </param>
        /// <param name='params'>
        /// <para>an array of parameters, terminated by type==0. It's left</para>
        /// <para>unspecified what happens with unknown parameters. At least</para>
        /// <para>MPV_RENDER_PARAM_API_TYPE is required, and most backends will</para>
        /// <para>require another backend-specific parameter.</para>
        /// </param>
        /// <returns>
        /// <para>error code, including but not limited to:</para>
        /// <para>MPV_ERROR_UNSUPPORTED: the OpenGL version is not supported</para>
        /// <para>(or required extensions are missing)</para>
        /// <para>MPV_ERROR_NOT_IMPLEMENTED: an unknown API type was provided, or</para>
        /// <para>support for the requested API was not</para>
        /// <para>built in the used libmpv binary.</para>
        /// <para>MPV_ERROR_INVALID_PARAMETER: at least one of the provided parameters was</para>
        /// <para>not valid.</para>
        /// </returns>
        [LibraryImport(LibraryName.Name, EntryPoint = "mpv_render_context_create", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvRenderContextCreate(global::LibMPVSharp.MpvRenderContext** res, global::LibMPVSharp.MpvHandle* mpv, global::LibMPVSharp.MpvRenderParam* @params);
        
        /// <summary>
        /// <para>Attempt to change a single parameter. Not all backends and parameter types</para>
        /// <para>support all kinds of changes.</para>
        /// </summary>
        /// <param name='ctx'>
        /// <para>a valid render context</para>
        /// </param>
        /// <param name='param'>
        /// <para>the parameter type and data that should be set</para>
        /// </param>
        /// <returns>
        /// <para>error code. If a parameter could actually be changed, this returns</para>
        /// <para>success, otherwise an error code depending on the parameter type</para>
        /// <para>and situation.</para>
        /// </returns>
        [LibraryImport(LibraryName.Name, EntryPoint = "mpv_render_context_set_parameter", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvRenderContextSetParameter(global::LibMPVSharp.MpvRenderContext* ctx, global::LibMPVSharp.MpvRenderParam param);
        
        /// <summary>
        /// <para>Retrieve information from the render context. This is NOT a counterpart to</para>
        /// <para>mpv_render_context_set_parameter(), because you generally can't read</para>
        /// <para>parameters set with it, and this function is not meant for this purpose.</para>
        /// <para>Instead, this is for communicating information from the renderer back to the</para>
        /// <para>user. See mpv_render_param_type; entries which support this function</para>
        /// <para>explicitly mention it, and for other entries you can assume it will fail.</para>
        /// <para>You pass param with param.type set and param.data pointing to a variable</para>
        /// <para>of the required data type. The function will then overwrite that variable</para>
        /// <para>with the returned value (at least on success).</para>
        /// </summary>
        /// <param name='ctx'>
        /// <para>a valid render context</para>
        /// </param>
        /// <param name='param'>
        /// <para>the parameter type and data that should be retrieved</para>
        /// </param>
        /// <returns>
        /// <para>error code. If a parameter could actually be retrieved, this returns</para>
        /// <para>success, otherwise an error code depending on the parameter type</para>
        /// <para>and situation. MPV_ERROR_NOT_IMPLEMENTED is used for unknown</para>
        /// <para>param.type, or if retrieving it is not supported.</para>
        /// </returns>
        [LibraryImport(LibraryName.Name, EntryPoint = "mpv_render_context_get_info", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvRenderContextGetInfo(global::LibMPVSharp.MpvRenderContext* ctx, global::LibMPVSharp.MpvRenderParam param);
        
        /// <summary>
        /// <para>Set the callback that notifies you when a new video frame is available, or</para>
        /// <para>if the video display configuration somehow changed and requires a redraw.</para>
        /// <para>Similar to mpv_set_wakeup_callback(), you must not call any mpv API from</para>
        /// <para>the callback, and all the other listed restrictions apply (such as not</para>
        /// <para>exiting the callback by throwing exceptions).</para>
        /// <para>This can be called from any thread, except from an update callback. In case</para>
        /// <para>of the OpenGL backend, no OpenGL state or API is accessed.</para>
        /// <para>Calling this will raise an update callback immediately.</para>
        /// </summary>
        /// <param name='callback'>
        /// <para>callback(callback_ctx) is called if the frame should be</para>
        /// <para>redrawn</para>
        /// </param>
        /// <param name='callback_ctx'>
        /// <para>opaque argument to the callback</para>
        /// </param>
        [LibraryImport(LibraryName.Name, EntryPoint = "mpv_render_context_set_update_callback", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void MpvRenderContextSetUpdateCallback(global::LibMPVSharp.MpvRenderContext* ctx, global::LibMPVSharp.MpvRenderUpdateFn callback, void* callback_ctx);
        
        /// <summary>
        /// <para>The API user is supposed to call this when the update callback was invoked</para>
        /// <para>(like all mpv_render_* functions, this has to happen on the render thread,</para>
        /// <para>and _not_ from the update callback itself).</para>
        /// <para>This is optional if MPV_RENDER_PARAM_ADVANCED_CONTROL was not set (default).</para>
        /// <para>Otherwise, it's a hard requirement that this is called after each update</para>
        /// <para>callback. If multiple update callback happened, and the function could not</para>
        /// <para>be called sooner, it's OK to call it once after the last callback.</para>
        /// <para>If an update callback happens during or after this function, the function</para>
        /// <para>must be called again at the soonest possible time.</para>
        /// <para>If MPV_RENDER_PARAM_ADVANCED_CONTROL was set, this will do additional work</para>
        /// <para>such as allocating textures for the video decoder.</para>
        /// </summary>
        /// <returns>
        /// <para>a bitset of mpv_render_update_flag values (i.e. multiple flags are</para>
        /// <para>combined with bitwise or). Typically, this will tell the API user</para>
        /// <para>what should happen next. E.g. if the MPV_RENDER_UPDATE_FRAME flag is</para>
        /// <para>set, mpv_render_context_render() should be called. If flags unknown</para>
        /// <para>to the API user are set, or if the return value is 0, nothing needs</para>
        /// <para>to be done.</para>
        /// </returns>
        [LibraryImport(LibraryName.Name, EntryPoint = "mpv_render_context_update", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial ulong MpvRenderContextUpdate(global::LibMPVSharp.MpvRenderContext* ctx);
        
        /// <summary>
        /// <para>Render video.</para>
        /// <para>Typically renders the video to a target surface provided via mpv_render_param</para>
        /// <para>(the details depend on the backend in use). Options like "panscan" are</para>
        /// <para>applied to determine which part of the video should be visible and how the</para>
        /// <para>video should be scaled. You can change these options at runtime by using the</para>
        /// <para>mpv property API.</para>
        /// <para>The renderer will reconfigure itself every time the target surface</para>
        /// <para>configuration (such as size) is changed.</para>
        /// <para>This function implicitly pulls a video frame from the internal queue and</para>
        /// <para>renders it. If no new frame is available, the previous frame is redrawn.</para>
        /// <para>The update callback set with mpv_render_context_set_update_callback()</para>
        /// <para>notifies you when a new frame was added. The details potentially depend on</para>
        /// <para>the backends and the provided parameters.</para>
        /// <para>Generally, libmpv will invoke your update callback some time before the video</para>
        /// <para>frame should be shown, and then lets this function block until the supposed</para>
        /// <para>display time. This will limit your rendering to video FPS. You can prevent</para>
        /// <para>this by setting the "video-timing-offset" global option to 0. (This applies</para>
        /// <para>only to "audio" video sync mode.)</para>
        /// <para>You should pass the following parameters:</para>
        /// <para>- Backend-specific target object, such as MPV_RENDER_PARAM_OPENGL_FBO.</para>
        /// <para>- Possibly transformations, such as MPV_RENDER_PARAM_FLIP_Y.</para>
        /// </summary>
        /// <param name='ctx'>
        /// <para>a valid render context</para>
        /// </param>
        /// <param name='params'>
        /// <para>an array of parameters, terminated by type==0. Which parameters</para>
        /// <para>are required depends on the backend. It's left unspecified what</para>
        /// <para>happens with unknown parameters.</para>
        /// </param>
        /// <returns>error code</returns>
        [LibraryImport(LibraryName.Name, EntryPoint = "mpv_render_context_render", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial int MpvRenderContextRender(global::LibMPVSharp.MpvRenderContext* ctx, global::LibMPVSharp.MpvRenderParam* @params);
        
        /// <summary>
        /// <para>Tell the renderer that a frame was flipped at the given time. This is</para>
        /// <para>optional, but can help the player to achieve better timing.</para>
        /// <para>Note that calling this at least once informs libmpv that you will use this</para>
        /// <para>function. If you use it inconsistently, expect bad video playback.</para>
        /// <para>If this is called while no video is initialized, it is ignored.</para>
        /// </summary>
        /// <param name='ctx'>
        /// <para>a valid render context</para>
        /// </param>
        [LibraryImport(LibraryName.Name, EntryPoint = "mpv_render_context_report_swap", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void MpvRenderContextReportSwap(global::LibMPVSharp.MpvRenderContext* ctx);
        
        /// <summary>
        /// <para>Destroy the mpv renderer state.</para>
        /// <para>If video is still active (e.g. a file playing), video will be disabled</para>
        /// <para>forcefully.</para>
        /// </summary>
        /// <param name='ctx'>
        /// <para>a valid render context. After this function returns, this is not</para>
        /// <para>a valid pointer anymore. NULL is also allowed and does nothing.</para>
        /// </param>
        [LibraryImport(LibraryName.Name, EntryPoint = "mpv_render_context_free", StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void MpvRenderContextFree(global::LibMPVSharp.MpvRenderContext* ctx);
        
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvRenderContext
    {
    }
    
    /// <summary>
    /// <para>Used to pass arbitrary parameters to some mpv_render_* functions. The</para>
    /// <para>meaning of the data parameter is determined by the type, and each</para>
    /// <para>MPV_RENDER_PARAM_* documents what type the value must point to.</para>
    /// <para>Each value documents the required data type as the pointer you cast to</para>
    /// <para>void* and set on mpv_render_param.data. For example, if MPV_RENDER_PARAM_FOO</para>
    /// <para>documents the type as Something* , then the code should look like this:</para>
    /// <para>Something foo = {...};</para>
    /// <para>mpv_render_param param;</para>
    /// <para>param.type = MPV_RENDER_PARAM_FOO;</para>
    /// <para>param.data =</para>
    /// <para>&</para>
    /// <para>foo;</para>
    /// <para>Normally, the data field points to exactly 1 object. If the type is char*,</para>
    /// <para>it points to a 0-terminated string.</para>
    /// <para>In all cases (unless documented otherwise) the pointers need to remain</para>
    /// <para>valid during the call only. Unless otherwise documented, the API functions</para>
    /// <para>will not write to the params array or any data pointed to it.</para>
    /// <para>As a convention, parameter arrays are always terminated by type==0. There</para>
    /// <para>is no specific order of the parameters required. The order of the 2 fields in</para>
    /// <para>this struct is guaranteed (even after ABI changes).</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvRenderParam
    {
        public global::LibMPVSharp.MpvRenderParamType type;
        public void* data;
    }
    
    /// <summary>
    /// <para>Information about the next video frame that will be rendered. Can be</para>
    /// <para>retrieved with MPV_RENDER_PARAM_NEXT_FRAME_INFO.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MpvRenderFrameInfo
    {
        /// <summary>
        /// <para>A bitset of mpv_render_frame_info_flag values (i.e. multiple flags are</para>
        /// <para>combined with bitwise or).</para>
        /// </summary>
        public ulong flags;
        /// <summary>
        /// <para>Absolute time at which the frame is supposed to be displayed. This is in</para>
        /// <para>the same unit and base as the time returned by mpv_get_time_us(). For</para>
        /// <para>frames that are redrawn, or if vsync locked video timing is used (see</para>
        /// <para>"video-sync" option), then this can be 0. The "video-timing-offset"</para>
        /// <para>option determines how much "headroom" the render thread gets (but a high</para>
        /// <para>enough frame rate can reduce it anyway). mpv_render_context_render() will</para>
        /// <para>normally block until the time is elapsed, unless you pass it</para>
        /// <para>MPV_RENDER_PARAM_BLOCK_FOR_TARGET_TIME = 0.</para>
        /// </summary>
        public long target_time;
    }
    
    
    /// <summary>
    /// <para>Parameters for mpv_render_param (which is used in a few places such as</para>
    /// <para>mpv_render_context_create().</para>
    /// <para>Also see mpv_render_param for conventions and how to use it.</para>
    /// </summary>
    public enum MpvRenderParamType
    {
        /// <summary>
        /// <para>Not a valid value, but also used to terminate a params array. Its value</para>
        /// <para>is always guaranteed to be 0 (even if the ABI changes in the future).</para>
        /// </summary>
        MPV_RENDER_PARAM_INVALID = 0,
        /// <summary>
        /// <para>The render API to use. Valid for mpv_render_context_create().</para>
        /// <para>Type: char*</para>
        /// <para>Defined APIs:</para>
        /// <para>MPV_RENDER_API_TYPE_OPENGL:</para>
        /// <para>OpenGL desktop 2.1 or later (preferably core profile compatible to</para>
        /// <para>OpenGL 3.2), or OpenGLES 2.0 or later.</para>
        /// <para>Providing MPV_RENDER_PARAM_OPENGL_INIT_PARAMS is required.</para>
        /// <para>It is expected that an OpenGL context is valid and "current" when</para>
        /// <para>calling mpv_render_* functions (unless specified otherwise). It</para>
        /// <para>must be the same context for the same mpv_render_context.</para>
        /// </summary>
        MPV_RENDER_PARAM_API_TYPE = 1,
        /// <summary>
        /// <para>Required parameters for initializing the OpenGL renderer. Valid for</para>
        /// <para>mpv_render_context_create().</para>
        /// <para>Type: mpv_opengl_init_params*</para>
        /// </summary>
        MPV_RENDER_PARAM_OPENGL_INIT_PARAMS = 2,
        /// <summary>
        /// <para>Describes a GL render target. Valid for mpv_render_context_render().</para>
        /// <para>Type: mpv_opengl_fbo*</para>
        /// </summary>
        MPV_RENDER_PARAM_OPENGL_FBO = 3,
        /// <summary>
        /// <para>Control flipped rendering. Valid for mpv_render_context_render().</para>
        /// <para>Type: int*</para>
        /// <para>If the value is set to 0, render normally. Otherwise, render it flipped,</para>
        /// <para>which is needed e.g. when rendering to an OpenGL default framebuffer</para>
        /// <para>(which has a flipped coordinate system).</para>
        /// </summary>
        MPV_RENDER_PARAM_FLIP_Y = 4,
        /// <summary>
        /// <para>Control surface depth. Valid for mpv_render_context_render().</para>
        /// <para>Type: int*</para>
        /// <para>This implies the depth of the surface passed to the render function in</para>
        /// <para>bits per channel. If omitted or set to 0, the renderer will assume 8.</para>
        /// <para>Typically used to control dithering.</para>
        /// </summary>
        MPV_RENDER_PARAM_DEPTH = 5,
        /// <summary>
        /// <para>ICC profile blob. Valid for mpv_render_context_set_parameter().</para>
        /// <para>Type: mpv_byte_array*</para>
        /// <para>Set an ICC profile for use with the "icc-profile-auto" option. (If the</para>
        /// <para>option is not enabled, the ICC data will not be used.)</para>
        /// </summary>
        MPV_RENDER_PARAM_ICC_PROFILE = 6,
        /// <summary>
        /// <para>Ambient light in lux. Valid for mpv_render_context_set_parameter().</para>
        /// <para>Type: int*</para>
        /// <para>This can be used for automatic gamma correction.</para>
        /// </summary>
        MPV_RENDER_PARAM_AMBIENT_LIGHT = 7,
        /// <summary>
        /// <para>X11 Display, sometimes used for hwdec. Valid for</para>
        /// <para>mpv_render_context_create(). The Display must stay valid for the lifetime</para>
        /// <para>of the mpv_render_context.</para>
        /// <para>Type: Display*</para>
        /// </summary>
        MPV_RENDER_PARAM_X11DISPLAY = 8,
        /// <summary>
        /// <para>Wayland display, sometimes used for hwdec. Valid for</para>
        /// <para>mpv_render_context_create(). The wl_display must stay valid for the</para>
        /// <para>lifetime of the mpv_render_context.</para>
        /// <para>Type: struct wl_display*</para>
        /// </summary>
        MPV_RENDER_PARAM_WL_DISPLAY = 9,
        /// <summary>
        /// <para>Better control about rendering and enabling some advanced features. Valid</para>
        /// <para>for mpv_render_context_create().</para>
        /// <para>This conflates multiple requirements the API user promises to abide if</para>
        /// <para>this option is enabled:</para>
        /// <para>- The API user's render thread, which is calling the mpv_render_*()</para>
        /// <para>functions, never waits for the core. Otherwise deadlocks can happen.</para>
        /// <para>See "Threading" section.</para>
        /// <para>- The callback set with mpv_render_context_set_update_callback() can now</para>
        /// <para>be called even if there is no new frame. The API user should call the</para>
        /// <para>mpv_render_context_update() function, and interpret the return value</para>
        /// <para>for whether a new frame should be rendered.</para>
        /// <para>- Correct functionality is impossible if the update callback is not set,</para>
        /// <para>or not set soon enough after mpv_render_context_create() (the core can</para>
        /// <para>block while waiting for you to call mpv_render_context_update(), and</para>
        /// <para>if the update callback is not correctly set, it will deadlock, or</para>
        /// <para>block for too long).</para>
        /// <para>In general, setting this option will enable the following features (and</para>
        /// <para>possibly more):</para>
        /// <para>- "Direct rendering", which means the player decodes directly to a</para>
        /// <para>texture, which saves a copy per video frame ("vd-lavc-dr" option</para>
        /// <para>needs to be enabled, and the rendering backend as well as the</para>
        /// <para>underlying GPU API/driver needs to have support for it).</para>
        /// <para>- Rendering screenshots with the GPU API if supported by the backend</para>
        /// <para>(instead of using a suboptimal software fallback via libswscale).</para>
        /// <para>Warning: do not just add this without reading the "Threading" section</para>
        /// <para>above, and then wondering that deadlocks happen. The</para>
        /// <para>requirements are tricky. But also note that even if advanced</para>
        /// <para>control is disabled, not adhering to the rules will lead to</para>
        /// <para>playback problems. Enabling advanced controls simply makes</para>
        /// <para>violating these rules fatal.</para>
        /// <para>Type: int*: 0 for disable (default), 1 for enable</para>
        /// </summary>
        MPV_RENDER_PARAM_ADVANCED_CONTROL = 10,
        /// <summary>
        /// <para>Return information about the next frame to render. Valid for</para>
        /// <para>mpv_render_context_get_info().</para>
        /// <para>Type: mpv_render_frame_info*</para>
        /// <para>It strictly returns information about the _next_ frame. The implication</para>
        /// <para>is that e.g. mpv_render_context_update()'s return value will have</para>
        /// <para>MPV_RENDER_UPDATE_FRAME set, and the user is supposed to call</para>
        /// <para>mpv_render_context_render(). If there is no next frame, then the</para>
        /// <para>return value will have is_valid set to 0.</para>
        /// </summary>
        MPV_RENDER_PARAM_NEXT_FRAME_INFO = 11,
        /// <summary>
        /// <para>Enable or disable video timing. Valid for mpv_render_context_render().</para>
        /// <para>Type: int*: 0 for disable, 1 for enable (default)</para>
        /// <para>When video is timed to audio, the player attempts to render video a bit</para>
        /// <para>ahead, and then do a blocking wait until the target display time is</para>
        /// <para>reached. This blocks mpv_render_context_render() for up to the amount</para>
        /// <para>specified with the "video-timing-offset" global option. You can set</para>
        /// <para>this parameter to 0 to disable this kind of waiting. If you do, it's</para>
        /// <para>recommended to use the target time value in mpv_render_frame_info to</para>
        /// <para>wait yourself, or to set the "video-timing-offset" to 0 instead.</para>
        /// <para>Disabling this without doing anything in addition will result in A/V sync</para>
        /// <para>being slightly off.</para>
        /// </summary>
        MPV_RENDER_PARAM_BLOCK_FOR_TARGET_TIME = 12,
        /// <summary>
        /// <para>Use to skip rendering in mpv_render_context_render().</para>
        /// <para>Type: int*: 0 for rendering (default), 1 for skipping</para>
        /// <para>If this is set, you don't need to pass a target surface to the render</para>
        /// <para>function (and if you do, it's completely ignored). This can still call</para>
        /// <para>into the lower level APIs (i.e. if you use OpenGL, the OpenGL context</para>
        /// <para>must be set).</para>
        /// <para>Be aware that the render API will consider this frame as having been</para>
        /// <para>rendered. All other normal rules also apply, for example about whether</para>
        /// <para>you have to call mpv_render_context_report_swap(). It also does timing</para>
        /// <para>in the same way.</para>
        /// </summary>
        MPV_RENDER_PARAM_SKIP_RENDERING = 13,
        /// <summary>
        /// <para>Deprecated. Not supported. Use MPV_RENDER_PARAM_DRM_DISPLAY_V2 instead.</para>
        /// <para>Type : struct mpv_opengl_drm_params*</para>
        /// </summary>
        MPV_RENDER_PARAM_DRM_DISPLAY = 14,
        /// <summary>
        /// <para>DRM draw surface size, contains draw surface dimensions.</para>
        /// <para>Valid for mpv_render_context_create().</para>
        /// <para>Type : struct mpv_opengl_drm_draw_surface_size*</para>
        /// </summary>
        MPV_RENDER_PARAM_DRM_DRAW_SURFACE_SIZE = 15,
        /// <summary>
        /// <para>DRM display, contains drm display handles.</para>
        /// <para>Valid for mpv_render_context_create().</para>
        /// <para>Type : struct mpv_opengl_drm_params_v2*</para>
        /// </summary>
        MPV_RENDER_PARAM_DRM_DISPLAY_V2 = 16,
        /// <summary>
        /// <para>MPV_RENDER_API_TYPE_SW only: rendering target surface size, mandatory.</para>
        /// <para>Valid for MPV_RENDER_API_TYPE_SW</para>
        /// <para>&</para>
        /// <para>mpv_render_context_render().</para>
        /// <para>Type: int[2] (e.g.: int s[2] = {w, h}; param.data =</para>
        /// <para>&s</para>
        /// <para>[0];)</para>
        /// <para>The video frame is transformed as with other VOs. Typically, this means</para>
        /// <para>the video gets scaled and black bars are added if the video size or</para>
        /// <para>aspect ratio mismatches with the target size.</para>
        /// </summary>
        MPV_RENDER_PARAM_SW_SIZE = 17,
        /// <summary>
        /// <para>MPV_RENDER_API_TYPE_SW only: rendering target surface pixel format,</para>
        /// <para>mandatory.</para>
        /// <para>Valid for MPV_RENDER_API_TYPE_SW</para>
        /// <para>&</para>
        /// <para>mpv_render_context_render().</para>
        /// <para>Type: char* (e.g.: char *f = "rgb0"; param.data = f;)</para>
        /// <para>Valid values are:</para>
        /// <para>"rgb0", "bgr0", "0bgr", "0rgb"</para>
        /// <para>4 bytes per pixel RGB, 1 byte (8 bit) per component, component bytes</para>
        /// <para>with increasing address from left to right (e.g. "rgb0" has r at</para>
        /// <para>address 0), the "0" component contains uninitialized garbage (often</para>
        /// <para>the value 0, but not necessarily; the bad naming is inherited from</para>
        /// <para>FFmpeg)</para>
        /// <para>Pixel alignment size: 4 bytes</para>
        /// <para>"rgb24"</para>
        /// <para>3 bytes per pixel RGB. This is strongly discouraged because it is</para>
        /// <para>very slow.</para>
        /// <para>Pixel alignment size: 1 bytes</para>
        /// <para>other</para>
        /// <para>The API may accept other pixel formats, using mpv internal format</para>
        /// <para>names, as long as it's internally marked as RGB, has exactly 1</para>
        /// <para>plane, and is supported as conversion output. It is not a good idea</para>
        /// <para>to rely on any of these. Their semantics and handling could change.</para>
        /// </summary>
        MPV_RENDER_PARAM_SW_FORMAT = 18,
        /// <summary>
        /// <para>MPV_RENDER_API_TYPE_SW only: rendering target surface bytes per line,</para>
        /// <para>mandatory.</para>
        /// <para>Valid for MPV_RENDER_API_TYPE_SW</para>
        /// <para>&</para>
        /// <para>mpv_render_context_render().</para>
        /// <para>Type: size_t*</para>
        /// <para>This is the number of bytes between a pixel (x, y) and (x, y + 1) on the</para>
        /// <para>target surface. It must be a multiple of the pixel size, and have space</para>
        /// <para>for the surface width as specified by MPV_RENDER_PARAM_SW_SIZE.</para>
        /// <para>Both stride and pointer value should be a multiple of 64 to facilitate</para>
        /// <para>fast SIMD operation. Lower alignment might trigger slower code paths,</para>
        /// <para>and in the worst case, will copy the entire target frame. If mpv is built</para>
        /// <para>with zimg (and zimg is not disabled), the performance impact might be</para>
        /// <para>less.</para>
        /// <para>In either cases, the pointer and stride must be aligned at least to the</para>
        /// <para>pixel alignment size. Otherwise, crashes and undefined behavior is</para>
        /// <para>possible on platforms which do not support unaligned accesses (either</para>
        /// <para>through normal memory access or aligned SIMD memory access instructions).</para>
        /// </summary>
        MPV_RENDER_PARAM_SW_STRIDE = 19,
        /// <summary>
        /// <para>MPV_RENDER_API_TYPE_SW only: rendering target surface bytes per line,</para>
        /// <para>mandatory.</para>
        /// <para>Valid for MPV_RENDER_API_TYPE_SW</para>
        /// <para>&</para>
        /// <para>mpv_render_context_render().</para>
        /// <para>Type: size_t*</para>
        /// <para>This is the number of bytes between a pixel (x, y) and (x, y + 1) on the</para>
        /// <para>target surface. It must be a multiple of the pixel size, and have space</para>
        /// <para>for the surface width as specified by MPV_RENDER_PARAM_SW_SIZE.</para>
        /// <para>Both stride and pointer value should be a multiple of 64 to facilitate</para>
        /// <para>fast SIMD operation. Lower alignment might trigger slower code paths,</para>
        /// <para>and in the worst case, will copy the entire target frame. If mpv is built</para>
        /// <para>with zimg (and zimg is not disabled), the performance impact might be</para>
        /// <para>less.</para>
        /// <para>In either cases, the pointer and stride must be aligned at least to the</para>
        /// <para>pixel alignment size. Otherwise, crashes and undefined behavior is</para>
        /// <para>possible on platforms which do not support unaligned accesses (either</para>
        /// <para>through normal memory access or aligned SIMD memory access instructions).</para>
        /// </summary>
        MPV_RENDER_PARAM_SW_POINTER = 20
    }
    
    /// <summary>
    /// <para>Flags used in mpv_render_frame_info.flags. Each value represents a bit in it.</para>
    /// </summary>
    [Flags]
    public enum MpvRenderFrameInfoFlag
    {
        /// <summary>
        /// <para>Set if there is actually a next frame. If unset, there is no next frame</para>
        /// <para>yet, and other flags and fields that require a frame to be queued will</para>
        /// <para>be unset.</para>
        /// <para>This is set for _any_ kind of frame, even for redraw requests.</para>
        /// <para>Note that when this is unset, it simply means no new frame was</para>
        /// <para>decoded/queued yet, not necessarily that the end of the video was</para>
        /// <para>reached. A new frame can be queued after some time.</para>
        /// <para>If the return value of mpv_render_context_render() had the</para>
        /// <para>MPV_RENDER_UPDATE_FRAME flag set, this flag will usually be set as well,</para>
        /// <para>unless the frame is rendered, or discarded by other asynchronous events.</para>
        /// </summary>
        MPV_RENDER_FRAME_INFO_PRESENT = 1,
        /// <summary>
        /// <para>If set, the frame is not an actual new video frame, but a redraw request.</para>
        /// <para>For example if the video is paused, and an option that affects video</para>
        /// <para>rendering was changed (or any other reason), an update request can be</para>
        /// <para>issued and this flag will be set.</para>
        /// <para>Typically, redraw frames will not be subject to video timing.</para>
        /// <para>Implies MPV_RENDER_FRAME_INFO_PRESENT.</para>
        /// </summary>
        MPV_RENDER_FRAME_INFO_REDRAW = 2,
        /// <summary>
        /// <para>If set, this is supposed to reproduce the previous frame perfectly. This</para>
        /// <para>is usually used for certain "video-sync" options ("display-..." modes).</para>
        /// <para>Typically the renderer will blit the video from a FBO. Unset otherwise.</para>
        /// <para>Implies MPV_RENDER_FRAME_INFO_PRESENT.</para>
        /// </summary>
        MPV_RENDER_FRAME_INFO_REPEAT = 4,
        /// <summary>
        /// <para>If set, the player timing code expects that the user thread blocks on</para>
        /// <para>vsync (by either delaying the render call, or by making a call to</para>
        /// <para>mpv_render_context_report_swap() at vsync time).</para>
        /// <para>Implies MPV_RENDER_FRAME_INFO_PRESENT.</para>
        /// </summary>
        MPV_RENDER_FRAME_INFO_BLOCK_VSYNC = 8
    }
    
    /// <summary>
    /// <para>Flags returned by mpv_render_context_update(). Each value represents a bit</para>
    /// <para>in the function's return value.</para>
    /// </summary>
    public enum MpvRenderUpdateFlag
    {
        /// <summary>
        /// <para>A new video frame must be rendered. mpv_render_context_render() must be</para>
        /// <para>called.</para>
        /// </summary>
        MPV_RENDER_UPDATE_FRAME = 1
    }
    
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void MpvRenderUpdateFn(void* cb_ctx);
    
}
