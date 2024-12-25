using Silk.NET.OpenGL;
using Silk.NET.WGL.Extensions.NV;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void ReferenceIncrement()
        {
            Interlocked.Increment(ref _referenceCount);
        }

        public void Dispose()
        {
            if (Interlocked.Decrement(ref _referenceCount) <= 0)
            {
                GL.Dispose();
                NVDXInterop.Dispose();
                Disposed = true;
            }

        }
    }
}
