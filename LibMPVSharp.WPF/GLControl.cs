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
        public static readonly DependencyProperty GLVersionProperty = DependencyProperty.Register(nameof(GLVersion), typeof(Version), typeof(GLControl), new FrameworkPropertyMetadata(new Version(3,2), PropertyChanged));
        public Version GLVersion
        {
            get => (Version)GetValue(GLVersionProperty);
            set => SetValue(GLVersionProperty, value);
        }

        private static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var glControl = d as GLControl;
            if (glControl == null) return;

            if (e.Property == GLVersionProperty)
            {
                var oldValue = e.OldValue as Version;
                var newValue = e.NewValue as Version;

                if (oldValue != null)
                {
                    glControl._renderContext?.Dispose();
                    glControl.DXGLContext?.Dispose();
                }

                if(newValue != null)
                {
                    glControl._dXGLContext = new DXGLContext(newValue);
                    glControl._renderContext = new RenderContext(glControl._dXGLContext);
                    glControl.OnDXGLChanged(glControl._dXGLContext);
                }
            }
        }

        private TimeSpan _lastRenderTime;
        private long _frameUpdated;

        private RenderContext? _renderContext;
        private DrawingVisual _drawingVisual;
        private DXGLContext? _dXGLContext;
        protected DXGLContext? DXGLContext => _dXGLContext;

        public GLControl()
        {
            _drawingVisual = new DrawingVisual();
            
            this.Loaded += GLControl_Loaded;
            this.Unloaded += GLControl_Unloaded;
        }

        public RenderContext? GLRenderContext => _renderContext;

        protected override int VisualChildrenCount => _drawingVisual == null ? 0 : 1;
        protected override Visual GetVisualChild(int index)
        {
            return _drawingVisual;
        }

        private void GLControl_Loaded(object sender, RoutedEventArgs e)
        {
            AddVisualChild(_drawingVisual);
            AddLogicalChild(_drawingVisual);

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (_dXGLContext == null || _dXGLContext.Disposed)
                {
                    _dXGLContext = new DXGLContext(GLVersion);
                }
                _renderContext = new RenderContext(_dXGLContext);
            }
        }

        private void GLControl_Unloaded(object sender, RoutedEventArgs e)
        {
            RemoveVisualChild(_drawingVisual);
            RemoveLogicalChild(_drawingVisual);

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                _renderContext?.Dispose();
                _renderContext = null;
                _dXGLContext?.Dispose();

            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == IsVisibleProperty)
            {
                if ((bool)e.NewValue)
                {
                    CompositionTarget.Rendering += CompositionTarget_Rendering;
                }
                else
                {
                    CompositionTarget.Rendering -= CompositionTarget_Rendering;
                }
            }
        }

        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            var args = (RenderingEventArgs)e;
            var frameUpdated = Interlocked.Read(ref _frameUpdated);
            Debug.WriteLine($"FrameUpdate = {frameUpdated}");
            if (_lastRenderTime != args.RenderingTime && frameUpdated > 0)
            {
                _lastRenderTime = args.RenderingTime;
                Interlocked.Decrement(ref _frameUpdated);

                FrameUpdate();
            }
        }


        public void RequestFrameUpdate()
        {
            Interlocked.Increment(ref _frameUpdated);
        }

        public void FrameUpdate()
        {
            if (DesignerProperties.GetIsInDesignMode(this) || _renderContext == null || _drawingVisual == null) return;

            var drawingContext = _drawingVisual.RenderOpen();

            var width = Math.Max(0, this.ActualWidth);
            var height = Math.Max(0, this.ActualHeight);
            if (_renderContext.BeginRender((uint)width, (uint)height))
            {
                OnDrawing();
                _renderContext.EndRender();
            }

            drawingContext.DrawImage(_renderContext.D3DImage, new Rect(0, 0, _renderContext.FrameBufferWidth, _renderContext.FrameBufferHeight));
            drawingContext.Close();
        }

        protected virtual void OnDrawing()
        {
            
        }

        protected virtual void OnDXGLChanged(DXGLContext dXGLContext)
        {

        }
    }
}
