using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LibMPVSharp.WPF
{
    public unsafe class VideoView : GLControl
    {
        public static readonly DependencyProperty MediaPlayerProperty = DependencyProperty.Register(nameof(MediaPlayer), typeof(MPVMediaPlayer), typeof(VideoView), new FrameworkPropertyMetadata(null, PropertyChagned));
        public MPVMediaPlayer? MediaPlayer
        {
            get => (MPVMediaPlayer)GetValue(MediaPlayerProperty);
            set => SetValue(MediaPlayerProperty, value);
        }

        private static void PropertyChagned(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var videoView = d as VideoView;
            if (videoView == null) return;

            if (e.Property == MediaPlayerProperty)
            {
                var oldValue = e.OldValue as MPVMediaPlayer;
                var newVlaue = e.NewValue as MPVMediaPlayer;

                if (oldValue != null)
                {
                    oldValue.Options.UpdateCallback = null;
                    oldValue.Options.GetProcAddress = null;
                }

                if (newVlaue != null)
                {
                    newVlaue.Options.GetProcAddress = videoView.GetProcAddress;
                    newVlaue.Options.UpdateCallback = videoView.OpenGLUpdateCallback;
                }
            }
        }

        protected override void OnDXGLChanged(DXGLContext dXGLContext)
        {
            if (MediaPlayer == null)
            {
                return;
            }

            MediaPlayer.SetProperty(MPVMediaPlayer.PlaybackControlOpts.Pause, true);
            MediaPlayer.ReleaseRenderContext();
            MediaPlayer.EnsureRenderContextCreated();
        }

        protected override void OnDrawing()
        {
            if (MediaPlayer == null || GLRenderContext == null) return;
            var width = (int)GLRenderContext.FrameBufferWidth;
            var height = (int)GLRenderContext.FrameBufferHeight;
            MediaPlayer.OpenGLRender(width, height, (int)GLRenderContext.GLFrameBuffer, (int)GLRenderContext.Format);
        }

        private IntPtr GetProcAddress(IntPtr ctx, string name)
        {
            if (DXGLContext?.GL?.Context == null)
            {
                return IntPtr.Zero;
            }
            
            return DXGLContext.GL.Context.GetProcAddress(name);
        }

        private void OpenGLUpdateCallback(void* ctx)
        {
            RequestFrameUpdate();
        }
    }
}
