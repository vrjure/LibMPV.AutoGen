using Avalonia;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;

namespace LibMPVSharp.Avalonia
{
    public unsafe class VideoView : OpenGlControlBase
    {
        public static readonly StyledProperty<MPVMediaPlayer?> MediaPlayerProperty = AvaloniaProperty.Register<VideoView, MPVMediaPlayer?>(nameof(MediaPlayer));
        public MPVMediaPlayer? MediaPlayer
        {
            get => GetValue(MediaPlayerProperty);
            set => SetValue(MediaPlayerProperty, value);
        }

        static VideoView()
        {
            MediaPlayerProperty.Changed.AddClassHandler<VideoView>((o, e) => o.OnPropertyChanged(e));
        }

        private MpvOpenglInitParams_get_proc_addressCallback? _getProcAddress;

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            if (MediaPlayer != null)
            {
                MediaPlayer.Options.UpdateCallback = OpenGLUpdateCallback;
                if (MediaPlayer.Options.SharedPlayer != null)
                {
                    MediaPlayer.Options.SharedPlayer.Options.UpdateCallback -= OpenGLUpdateCallback;
                    MediaPlayer.Options.SharedPlayer.Options.UpdateCallback += OpenGLUpdateCallback;
                }
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            if (MediaPlayer != null)
            {
                MediaPlayer.Options.UpdateCallback = null;
                if (MediaPlayer.Options.SharedPlayer != null)
                {
                    MediaPlayer.Options.SharedPlayer.Options.UpdateCallback -= OpenGLUpdateCallback;
                }
            }
        }


        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == MediaPlayerProperty)
            {
                var oldNew = change.GetOldAndNewValue<MPVMediaPlayer>();
                if (oldNew.oldValue != null)
                {
                    oldNew.oldValue.Options.GetProcAddress = null;
                    oldNew.oldValue.Options.UpdateCallback = null;
                    if (oldNew.newValue.Options.SharedPlayer != null)
                    {
                        oldNew.newValue.Options.SharedPlayer.Options.UpdateCallback -= OpenGLUpdateCallback;
                    }
                }

                if (oldNew.newValue != null)
                {
                    oldNew.newValue.Options.GetProcAddress = _getProcAddress;
                    oldNew.newValue.Options.UpdateCallback = OpenGLUpdateCallback;
                    if (oldNew.newValue.Options.SharedPlayer != null)
                    {
                        oldNew.newValue.Options.SharedPlayer.Options.UpdateCallback += OpenGLUpdateCallback;
                    }
                }
            }
        }

        protected override void OnOpenGlRender(GlInterface gl, int fb)
        {
            if (MediaPlayer == null) return;

            var scale = VisualRoot?.RenderScaling ?? 1d;
            var width = Bounds.Width * scale;
            var height = Bounds.Height * scale;
            MediaPlayer.OpenGLRender((int)width, (int)height, fb, flipY: 1);
        }

        protected override void OnOpenGlInit(GlInterface gl)
        {
            _getProcAddress = (ctx, name) => gl.GetProcAddress(name);
            if (MediaPlayer != null)
            {
                MediaPlayer.Options.GetProcAddress = _getProcAddress;
                MediaPlayer.EnsureRenderContextCreated();
            }
        }

        protected override void OnOpenGlDeinit(GlInterface gl)
        {
            if (MediaPlayer == null) return;
            MediaPlayer.ReleaseRenderContext();
        }

        private void OpenGLUpdateCallback(void* ctx)
        {
            Dispatcher.UIThread.InvokeAsync(this.RequestNextFrameRendering, DispatcherPriority.Background);
        }
    }
}
