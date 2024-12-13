using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibMPVSharp
{
    public unsafe partial class MPVMediaPlayer
    {
        private MpvRenderContext* _renderContext;
        private void EnsureRenderContextCreated()
        {
            CheckClientHandle();

            if (_options.GetProcAddress == null || _options.UpdateCallback == null) return;
            if (_renderContext == null)
            {
                var MPV_RENDER_PARAM_API_TYPE_Data = Marshal.StringToHGlobalAnsi("opengl");

                var openglInitParams = new MpvOpenglInitParams
                {
                    get_proc_address = _options.GetProcAddress,
                    get_proc_address_ctx = null
                };
                var MPV_RENDER_PARAM_OPENGL_INIT_PARAMS_Data = Marshal.AllocHGlobal(Marshal.SizeOf<MpvOpenglInitParams>());
                Marshal.StructureToPtr(openglInitParams, MPV_RENDER_PARAM_OPENGL_INIT_PARAMS_Data, false);

                var parameters = new MpvRenderParam[]
                {
                    new(){ type = MpvRenderParamType.MPV_RENDER_PARAM_API_TYPE, data = (void*)MPV_RENDER_PARAM_API_TYPE_Data},
                    new(){ type = MpvRenderParamType.MPV_RENDER_PARAM_OPENGL_INIT_PARAMS, data = (void*)MPV_RENDER_PARAM_OPENGL_INIT_PARAMS_Data},
                    new(){ type = MpvRenderParamType.MPV_RENDER_PARAM_INVALID, data = null}
                };

                try
                {
                    MpvRenderContext* content = null;
                    fixed (MpvRenderParam* ptr = parameters)
                    {
                        var result = Render.MpvRenderContextCreate(&content, _clientHandle, ptr);
                        CheckError(result, nameof(Render.MpvRenderContextCreate));
                    }
                    _renderContext = content;

                    Render.MpvRenderContextSetUpdateCallback(_renderContext, _options.UpdateCallback, null);
                }
                finally
                {
                    Marshal.FreeHGlobal(MPV_RENDER_PARAM_API_TYPE_Data);
                    Marshal.FreeHGlobal(MPV_RENDER_PARAM_OPENGL_INIT_PARAMS_Data);
                }
            }
        }

        public void OpenGLRender(int width, int height, int fbo, int format, int flipY = 0)
        {
            if (_renderContext == null) return;

            var fboArray = new MpvOpenglFbo[]
            {
                new(){ w = width, h = height, fbo = fbo, internal_format = format },
            };

            var flipYArray = new int[] { flipY };

            fixed(MpvOpenglFbo* fboPtr = fboArray)
            {
                fixed(int* flipYPtr = flipYArray)
                {
                    var parameters = new MpvRenderParam[]
                    {
                        new(){ type = MpvRenderParamType.MPV_RENDER_PARAM_OPENGL_FBO, data = fboPtr },
                        new(){ type = MpvRenderParamType.MPV_RENDER_PARAM_FLIP_Y, data = flipYPtr},
                        new(){ type = MpvRenderParamType.MPV_RENDER_PARAM_INVALID, data = null }
                    };

                    fixed(MpvRenderParam* renderParamPtr = parameters)
                    {
                        var result = Render.MpvRenderContextRender(_renderContext, renderParamPtr);
                        CheckError(result, nameof(Render.MpvRenderContextRender));
                    }
                }
            }
        }
    }
}
