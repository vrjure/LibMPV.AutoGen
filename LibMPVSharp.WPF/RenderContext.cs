using Silk.NET.Core.Native;
using Silk.NET.Direct3D9;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.WGL.Extensions.NV;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media;

namespace LibMPVSharp.WPF
{
    public unsafe class RenderContext : IDisposable
    {
        public GL GL { get; }
        public NVDXInterop NVDXInterop { get; }
        public IDirect3DDevice9Ex* DxDevice { get; }
        public IntPtr GLDevice { get; }
        public Format Format { get; }
        public D3DImage? D3DImage { get; private set; }
        public uint Width { get; private set; }
        public uint Height { get; private set; }
        public uint GLFrameBuffer => _glFrameBuffer;

        private IDirect3DSurface9* _dxSurface;
        private uint _glFrameBuffer;
        private uint _glSharedTexture;
        private uint _glDepthRenderBuffer;
        private IntPtr _dxRegisteredHandle;
        public RenderContext(Version glVersion)
        {
            IDirect3D9Ex* d3d9DeviceEx;
            D3D9.GetApi(null).Direct3DCreate9Ex(D3D9.SdkVersion, &d3d9DeviceEx);

            Displaymodeex displayMode = new((uint)sizeof(Displaymodeex));
            d3d9DeviceEx->GetAdapterDisplayModeEx(D3D9.AdapterDefault, ref displayMode, null);
            Format = displayMode.Format;

            PresentParameters presentParameters = new()
            {
                Windowed = 1,
                SwapEffect = Swapeffect.Discard,
                HDeviceWindow = 0,
                PresentationInterval = 0,
                BackBufferFormat = Format,
                BackBufferWidth = 1,
                BackBufferHeight = 1,
                AutoDepthStencilFormat = Format.Unknown,
                BackBufferCount = 1,
                EnableAutoDepthStencil = 0,
                Flags = 0,
                FullScreenRefreshRateInHz = 0,
                MultiSampleQuality = 0,
                MultiSampleType = MultisampleType.MultisampleNone
            };

            IDirect3DDevice9Ex* dxDevice = null;
            d3d9DeviceEx->CreateDeviceEx(D3D9.AdapterDefault, Devtype.Hal, 0, D3D9.CreateHardwareVertexprocessing | D3D9.CreateMultithreaded | D3D9.CreatePuredevice, ref presentParameters, (Displaymodeex*)IntPtr.Zero, &dxDevice);

            DxDevice = dxDevice;

            var windowOptions = WindowOptions.Default;
            windowOptions.API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Default, new APIVersion(glVersion));
            windowOptions.IsVisible = false;

            IWindow window = Window.Create(windowOptions);
            window.Initialize();
            GL = window.CreateOpenGL();
            NVDXInterop = new NVDXInterop(GL.Context);

            GLDevice = NVDXInterop.DxopenDevice(dxDevice);
        }


        public void DXGLBinding(uint width, uint height)
        {
            if (this.Width == width && this.Height == height)
            {
                return;
            }

            this.Width = width;
            this.Height = height;

            void* shareHandle = (void*)IntPtr.Zero;
            IDirect3DSurface9* surface;
            DxDevice->CreateRenderTarget(width, height, Format, MultisampleType.MultisampleNone, 0, 0, &surface, &shareHandle);

            NVDXInterop.DxsetResourceShareHandle(surface, (IntPtr)shareHandle);

            if (_glFrameBuffer == 0)
            {
                _glFrameBuffer = GL.GenFramebuffer();
            }

            if (_glSharedTexture == 0)
            {
                _glSharedTexture = GL.GenTexture();
            }

            if (_dxRegisteredHandle != 0)
            {
                NVDXInterop.DxunregisterObject(GLDevice, _dxRegisteredHandle);
            }
            _dxRegisteredHandle = NVDXInterop.DxregisterObject(GLDevice, surface, _glSharedTexture, (NV)TextureTarget.Texture2D, NV.AccessReadWriteNV);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _glFrameBuffer);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _glSharedTexture, 0);

            D3DImage = new D3DImage();
            D3DImage.Lock();
            D3DImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, (IntPtr)surface);
            D3DImage.Unlock();

            if (_dxSurface != null)
            {
                _dxSurface->Release();
            }
            _dxSurface = surface;

        }

        public void BeginRender()
        {
            if (D3DImage == null || _dxSurface == null) return;
            D3DImage.Lock();
            NVDXInterop.DxlockObjects(GLDevice, 1, [_dxRegisteredHandle]);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _glFrameBuffer);
            GL.Viewport(new Rectangle<int>(0, 0, (int)Width, (int)Height));
        }

        public void EndRender()
        {
            if (D3DImage == null || _dxSurface == null) return;

            NVDXInterop.DxunlockObjects(GLDevice, 1, [_dxRegisteredHandle]);
            D3DImage.AddDirtyRect(new System.Windows.Int32Rect(0, 0, (int)Width, (int)Height));
            D3DImage.Unlock();
        }

        public void Dispose()
        {
            if(_glFrameBuffer != 0)
                GL.DeleteBuffer(_glFrameBuffer);

            if(_glSharedTexture != 0)
                GL.DeleteTexture(_glSharedTexture);

            if(_glDepthRenderBuffer != 0)
                GL.DeleteRenderbuffer(_glDepthRenderBuffer);

            if(_dxRegisteredHandle != 0)
                NVDXInterop.DxunregisterObject(GLDevice, _dxRegisteredHandle);

            if(_dxSurface != null)
                _dxSurface->Release();

            if(DxDevice != null)
                DxDevice->Release();

            GL?.Dispose();
            NVDXInterop?.Dispose();
        }
    }
}
