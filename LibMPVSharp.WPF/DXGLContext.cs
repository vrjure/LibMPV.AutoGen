using Silk.NET.OpenGL;
using Silk.NET.WGL.Extensions.NV;
using Silk.NET.Windowing;

namespace LibMPVSharp.WPF
{
    public class DXGLContext : IDisposable
    {
        private int _referenceCount;
        public bool Disposed { get; private set; }
        public GL GL { get; set; }
        public NVDXInterop NVDXInterop { get; set; }

        public DXGLContext(Version glVersion)
        {
            if (glVersion == null)
                glVersion = new Version(3, 2);
            var windowOptions = WindowOptions.Default;
            windowOptions.API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Default, new APIVersion(glVersion));
            windowOptions.IsVisible = false;

            IWindow window = Window.Create(windowOptions);
            window.Initialize();
            GL = window.CreateOpenGL();
            NVDXInterop = new NVDXInterop(GL.Context);
        }


        public void Dispose()
        {
            if (!Disposed)
            {
                Disposed = true;
                GL.Dispose();
                NVDXInterop.Dispose();
            }


        }
    }
}
