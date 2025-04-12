using Silk.NET.Direct3D9;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.WGL.Extensions.NV;
using System.Windows.Interop;

namespace LibMPVSharp.WPF
{
    public unsafe class RenderContext : IDisposable
    {
        private readonly DXGLContext _dXGLContext;
        private GL GL => _dXGLContext.GL;
        private NVDXInterop NVDXInterop => _dXGLContext.NVDXInterop;

        public IDirect3DDevice9Ex* DxDevice { get; }
        public IntPtr GLDevice { get; }
        public Format Format { get; }
        public D3DImage? D3DImage { get; private set; }
        public uint FrameBufferWidth { get; private set; }
        public uint FrameBufferHeight { get; private set; }
        public uint GLFrameBuffer => _glFrameBuffer;

        private IDirect3DSurface9* _dxSurface;
        private uint _glFrameBuffer;
        private uint _glSharedTexture;
        private uint _glDepthRenderBuffer;
        private IntPtr _dxRegisteredHandle;

        public RenderContext(DXGLContext dXGLContext)
        {
            _dXGLContext = dXGLContext;

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
            GLDevice = NVDXInterop.DxopenDevice(dxDevice);
        }


        private void DXGLBindingIfNeed(uint width, uint height)
        {
            if (D3DImage != null && this.FrameBufferWidth == width && this.FrameBufferHeight == height)
            {
                return;
            }

            this.FrameBufferWidth = width;
            this.FrameBufferHeight = height;

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
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            if (_dxSurface != null)
            {
                _dxSurface->Release();
            }
            _dxSurface = surface;

            D3DImage ??= new D3DImage();
        }

        public bool BeginRender(uint width, uint height)
        {
            DXGLBindingIfNeed(width, height);

            if (D3DImage == null || _dxSurface == null || !D3DImage.IsFrontBufferAvailable) return false;

            D3DImage.Lock();
            D3DImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, (IntPtr)_dxSurface);

            NVDXInterop.DxlockObjects(GLDevice, 1, [_dxRegisteredHandle]);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _glFrameBuffer);
            GL.Viewport(new Rectangle<int>(0, 0, (int)FrameBufferWidth, (int)FrameBufferHeight));
            return true;
        }

        public void EndRender()
        {
            if (D3DImage == null || _dxSurface == null || !D3DImage.IsFrontBufferAvailable) return;

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            NVDXInterop.DxunlockObjects(GLDevice, 1, [_dxRegisteredHandle]);
            D3DImage.AddDirtyRect(new System.Windows.Int32Rect(0, 0, (int)FrameBufferWidth, (int)FrameBufferHeight));
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

            _dXGLContext.Dispose();
        }
    }
}
