using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Silk.NET.Windowing;
using Silk.NET.Core.Contexts;
using Silk.NET.GLFW;
using Silk.NET.Maths;
using Silk.NET.WGL.Extensions.NV;
using Silk.NET.WGL;
using System.IO;
using System.Diagnostics;
using System.Windows.Threading;
using System.Windows.Controls;

namespace LibMPVSharp.WPF
{
    public class GLControl: FrameworkElement
    {
        private readonly RenderContext? _renderContext;
        private DrawingVisual _drawingVisual;
        public GLControl()
        {
            if(!DesignerProperties.GetIsInDesignMode(this))
            {
                _renderContext = new RenderContext(new Version(3, 2));
            }
            _drawingVisual = new DrawingVisual();

            this.Loaded += GLControl_Loaded;
            this.Unloaded += GLControl_Unloaded;
        }

        private void GLControl_Loaded(object sender, RoutedEventArgs e)
        {
            AddVisualChild(_drawingVisual);
            AddLogicalChild(_drawingVisual);
        }

        private void GLControl_Unloaded(object sender, RoutedEventArgs e)
        {
            RemoveVisualChild(_drawingVisual);
            RemoveLogicalChild(_drawingVisual);
        }

        public RenderContext? GLRenderContext => _renderContext;

        protected override int VisualChildrenCount => _drawingVisual == null ? 0 : 1;
        protected override Visual GetVisualChild(int index)
        {
            return _drawingVisual;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if (DesignerProperties.GetIsInDesignMode(this) || _renderContext == null)
            {
                return;
            }
            
            _renderContext.DXGLBinding((uint)sizeInfo.NewSize.Width, (uint)sizeInfo.NewSize.Height);
            DrawFrame();
        }


        public void DrawFrame()
        {
            if (DesignerProperties.GetIsInDesignMode(this) || _drawingVisual == null || !(_renderContext?.D3DImage?.IsFrontBufferAvailable == true)) return;

            var drawingContext = _drawingVisual.RenderOpen();

            _renderContext.BeginRender();
            OnDrawing();
            _renderContext.EndRender();

            drawingContext.DrawImage(_renderContext.D3DImage, new Rect(0, 0, _renderContext.Width, _renderContext.Height));
            drawingContext.Close();
        }

        protected virtual void OnDrawing()
        {
            
        }
    }
}
