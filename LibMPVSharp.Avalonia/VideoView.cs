using Avalonia;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                }

                if (oldNew.newValue != null)
                {
                    oldNew.newValue.Options.GetProcAddress = _getProcAddress;
                    oldNew.newValue.Options.UpdateCallback = OpenGLUpdateCallback;
                }
            }
        }

        protected override void OnOpenGlRender(GlInterface gl, int fb)
        {
            if (MediaPlayer == null) return;

            var width = Bounds.Width;
            var height = Bounds.Height;
            MediaPlayer.OpenGLRender((int)width, (int)height, fb);
        }

        protected override void OnOpenGlInit(GlInterface gl)
        {
            _getProcAddress = (ctx, name) => gl.GetProcAddress(name);
            if (MediaPlayer != null)
            {
                MediaPlayer.Options.GetProcAddress = _getProcAddress;
            }
        }

        private void OpenGLUpdateCallback(void* ctx)
        {
            Dispatcher.UIThread.InvokeAsync(this.RequestNextFrameRendering, DispatcherPriority.Background);
        }
    }
}
