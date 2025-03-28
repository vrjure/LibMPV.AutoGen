using LibMPVSharp.Extensions;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LibMPVSharp.WPF.Demo
{
    public class MediaPlayerView : Control
    {
        public static readonly DependencyProperty MediaPlayerProperty = DependencyProperty.Register(nameof(MediaPlayer), typeof(MPVMediaPlayer), typeof(MediaPlayerView), new FrameworkPropertyMetadata(null, PropertyChagned));
        public MPVMediaPlayer? MediaPlayer
        {
            get => (MPVMediaPlayer)GetValue(MediaPlayerProperty);
            set => SetValue(MediaPlayerProperty, value);
        }

        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof(Duration), typeof(TimeSpan), typeof(MediaPlayerView), new FrameworkPropertyMetadata(TimeSpan.Zero));
        public TimeSpan Duration
        {
            get => (TimeSpan)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(nameof(Time), typeof(TimeSpan), typeof(MediaPlayerView), new FrameworkPropertyMetadata(TimeSpan.Zero, PropertyChagned));
        public TimeSpan Time
        {
            get => (TimeSpan)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register(nameof(Volume), typeof(long), typeof(MediaPlayerView), new FrameworkPropertyMetadata(0L, PropertyChagned));
        public long Volume
        {
            get => (long)GetValue(VolumeProperty);
            set => SetValue(VolumeProperty, value);
        }

        public static readonly DependencyProperty MaxVolumeProperty = DependencyProperty.Register(nameof(MaxVolume), typeof(long), typeof(MediaPlayerView), new FrameworkPropertyMetadata(1000L));
        public long MaxVolume
        {
            get => (long)GetValue(MaxVolumeProperty);
            set => SetValue(MaxVolumeProperty, value);
        }

        public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register(nameof(Speed), typeof(double), typeof(MediaPlayerView), new FrameworkPropertyMetadata(1d));
        public double Speed
        {
            get => (double)GetValue(SpeedProperty);
            set => SetValue(SpeedProperty, value);
        }

        public static readonly DependencyProperty PlayingProperty = DependencyProperty.Register(nameof(Playing), typeof(bool), typeof(MediaPlayerView), new PropertyMetadata(false));
        public bool Playing
        {
            get => (bool)GetValue(PlayingProperty);
            set => SetValue(PlayingProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(MediaPlayerView), new PropertyMetadata(""));
        public string? Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register(nameof(AspectRatio), typeof(string), typeof(MediaPlayerView), new PropertyMetadata("no", PropertyChagned));
        public string AspectRatio
        {
            get => (string)GetValue(AspectRatioProperty);
            set => SetValue(AspectRatioProperty, value);
        }

        public static readonly DependencyProperty VideoParamsProperty = DependencyProperty.Register(nameof(VideoParams), typeof(string), typeof(MediaPlayerView), new PropertyMetadata(""));
        public string VideoParams
        {
            get => (string)GetValue(VideoParamsProperty);
            set => SetValue(VideoParamsProperty, value);
        }

        private static void PropertyChagned(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as MediaPlayerView;
            if (view == null) return;

            if (e.Property == MediaPlayerProperty)
            {
                var oldValue = e.OldValue as MPVMediaPlayer;
                var newVlaue = e.NewValue as MPVMediaPlayer;

                if (oldValue != null)
                {
                    oldValue.MpvEvent -= view.OnMpvEvent;
                }

                if (newVlaue != null)
                {
                    newVlaue.ObservableProperty(MPVMediaPlayer.PlaybackControlOpts.Pause, MpvFormat.MPV_FORMAT_FLAG);
                    newVlaue.ObservableProperty(MPVMediaPlayer.Properties.Duration, MpvFormat.MPV_FORMAT_DOUBLE);
                    newVlaue.ObservableProperty(MPVMediaPlayer.Properties.TimePos, MpvFormat.MPV_FORMAT_DOUBLE);
                    newVlaue.ObservableProperty(MPVMediaPlayer.AudioOpts.Volume, MpvFormat.MPV_FORMAT_INT64);
                    newVlaue.ObservableProperty(MPVMediaPlayer.AudioOpts.Mute, MpvFormat.MPV_FORMAT_STRING);
                    newVlaue.ObservableProperty(MPVMediaPlayer.PlaybackControlOpts.Speed, MpvFormat.MPV_FORMAT_DOUBLE);

                    newVlaue.MpvEvent += view.OnMpvEvent;

                    view.SetCurrentValue(VolumeProperty, newVlaue.GetPropertyLong(MPVMediaPlayer.AudioOpts.Volume));
                    view.SetCurrentValue(MaxVolumeProperty, newVlaue.GetPropertyLong(MPVMediaPlayer.AudioOpts.VolumeMax));
                    view.SetCurrentValue(SpeedProperty, newVlaue.GetPropertyDouble(MPVMediaPlayer.PlaybackControlOpts.Speed));
                }
            }
            else if (e.Property == TimeProperty)
            {
                if (view.MediaPlayer == null) return;
                var oldValue = (TimeSpan)e.OldValue;
                var newValue = (TimeSpan)e.NewValue;
                if (Math.Abs(newValue.TotalSeconds -  oldValue.TotalSeconds) > 1)
                {
                    view.MediaPlayer.SetProperty(MPVMediaPlayer.Properties.TimePos, newValue.TotalSeconds);
                }
            }
            else if (e.Property == VolumeProperty)
            {
                if (view.MediaPlayer == null) return;
                var value = (long)e.NewValue;
                if (value != view.MediaPlayer.GetPropertyLong(MPVMediaPlayer.AudioOpts.Volume))
                {
                    view.MediaPlayer.SetProperty(MPVMediaPlayer.AudioOpts.Volume, value);
                }
            }
            else if (e.Property == AspectRatioProperty)
            {
                if (view.MediaPlayer == null) return;
                view.MediaPlayer.SetProperty(MPVMediaPlayer.VideoOpts.VideoAspectOverride, (string)e.NewValue);
            }
        }

        public static readonly RoutedCommand OpenFile;
        public static readonly RoutedCommand PlayPause;
        public static readonly RoutedCommand SpeedCmd;
        public static readonly RoutedCommand AspectRatioCmd;


        private static Queue<string> _aspectRatio = new Queue<string>();

        static MediaPlayerView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MediaPlayerView), new FrameworkPropertyMetadata(typeof(MediaPlayerView)));

            OpenFile = new RoutedCommand(nameof(OpenFile), typeof(MediaPlayerView));
            PlayPause = new RoutedCommand(nameof(PlayPause), typeof(MediaPlayerView));
            SpeedCmd = new RoutedCommand(nameof(SpeedCmd), typeof(MediaPlayerView));
            AspectRatioCmd = new RoutedCommand(nameof(AspectRatioCmd), typeof(MediaPlayerView));

            _aspectRatio.Enqueue("no");
            _aspectRatio.Enqueue("16:9");
            _aspectRatio.Enqueue("4:3");
        }

        public MediaPlayerView()
        {
            this.CommandBindings.Add(new CommandBinding(OpenFile, (s, e) => TryOpenFile()));
            this.CommandBindings.Add(new CommandBinding(PlayPause, (s, e) => TryPlayPause()));
            this.CommandBindings.Add(new CommandBinding(SpeedCmd, (s,e) => TrySwitchSpeed()));
            this.CommandBindings.Add(new CommandBinding(AspectRatioCmd, (s, e) => TrySwitchAspectRatio()));
        }


        private void OnMpvEvent(object? sender, MpvEvent mpvEvent)
        {
            switch (mpvEvent.event_id)
            {
                case MpvEventId.MPV_EVENT_NONE:
                    break;
                case MpvEventId.MPV_EVENT_SHUTDOWN:
                    break;
                case MpvEventId.MPV_EVENT_LOG_MESSAGE:
                    break;
                case MpvEventId.MPV_EVENT_GET_PROPERTY_REPLY:
                    break;
                case MpvEventId.MPV_EVENT_SET_PROPERTY_REPLY:
                    break;
                case MpvEventId.MPV_EVENT_COMMAND_REPLY:
                    break;
                case MpvEventId.MPV_EVENT_START_FILE:
                    break;
                case MpvEventId.MPV_EVENT_END_FILE:
                    break;
                case MpvEventId.MPV_EVENT_FILE_LOADED:
                    MpvFiledLoaded(sender);
                    break;
                case MpvEventId.MPV_EVENT_IDLE:
                    break;
                case MpvEventId.MPV_EVENT_TICK:
                    break;
                case MpvEventId.MPV_EVENT_CLIENT_MESSAGE:
                    break;
                case MpvEventId.MPV_EVENT_VIDEO_RECONFIG:
                    break;
                case MpvEventId.MPV_EVENT_AUDIO_RECONFIG:
                    break;
                case MpvEventId.MPV_EVENT_SEEK:
                    break;
                case MpvEventId.MPV_EVENT_PLAYBACK_RESTART:
                    break;
                case MpvEventId.MPV_EVENT_PROPERTY_CHANGE:
                    MPVPropertyChanged(sender, mpvEvent.ReadData<MpvEventProperty>());
                    break;
                case MpvEventId.MPV_EVENT_QUEUE_OVERFLOW:
                    break;
                case MpvEventId.MPV_EVENT_HOOK:
                    break;
                default:
                    break;
            }
        }

        private void MPVPropertyChanged(object? sender, MpvEventProperty property)
        {
            if (property.format == MpvFormat.MPV_FORMAT_NONE) return;

            if (property.name == "duration")
            {
                DispatchSetCurrentValue(DurationProperty, TimeSpan.FromSeconds(property.ReadDoubleValue()));
            }
            else if (property.name == "time-pos")
            {
                DispatchSetCurrentValue(TimeProperty, TimeSpan.FromSeconds(property.ReadDoubleValue()));
            }
            else if (property.name == "pause")
            {
                DispatchSetCurrentValue(PlayingProperty, !property.ReadBoolValue());
            }
            else if (property.name == "volume")
            {
                DispatchSetCurrentValue(VolumeProperty, property.ReadLongValue());
            }
            else if (property.name == "speed")
            {
                DispatchSetCurrentValue(SpeedProperty, property.ReadDoubleValue());
            }
        }

        private void MpvFiledLoaded(object? sender)
        {
            Dispatcher.BeginInvoke(TryGetVideoParams);
        }

        private void DispatchSetCurrentValue(DependencyProperty property, object value)
        {
            Dispatcher.BeginInvoke(() => SetCurrentValue(property, value));
        }

        private void TryOpenFile()
        {
            if (MediaPlayer == null)
            {
                return;
            }

            var dialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "mp4|*.mp4|all|*.*",
            };

            if(dialog.ShowDialog() ==  true)
            {
                var file = dialog.FileName;
                try
                {
                    MediaPlayer.EnsureRenderContextCreated();
                    MediaPlayer.ExecuteCommand("loadfile", file);
                }
                finally
                {
                    var pause = MediaPlayer.GetPropertyBoolean(MPVMediaPlayer.PlaybackControlOpts.Pause);
                    Playing = !pause;
                }
            }
        }

        private void TryPlayPause()
        {
            if (MediaPlayer == null) return;
            var pause = MediaPlayer.GetPropertyBoolean(MPVMediaPlayer.PlaybackControlOpts.Pause);
            MediaPlayer.SetProperty(MPVMediaPlayer.PlaybackControlOpts.Pause, !pause);
        }
        private void TrySwitchSpeed()
        {
            if (MediaPlayer == null) return;

            var speed = MediaPlayer.GetPropertyDouble(MPVMediaPlayer.PlaybackControlOpts.Speed);
            speed++;
            if (speed > 2)
            {
                MediaPlayer.SetProperty(MPVMediaPlayer.PlaybackControlOpts.Speed, 1d);
            }
            else
            {
                MediaPlayer.SetProperty(MPVMediaPlayer.PlaybackControlOpts.Speed, speed);
            }
        }

        private void TrySwitchAspectRatio()
        {
            if (MediaPlayer == null) return;
            var ratio = _aspectRatio.Dequeue();
            _aspectRatio.Enqueue(ratio);
            AspectRatio = ratio;
        }

        private void TryGetVideoParams()
        {
            if (MediaPlayer == null) return;
            var node = MediaPlayer.GetPropertyNode("video-params");
            using var sw = new StringWriter();
            using var writer = new IndentedTextWriter(sw);
            node.Node.ReadToWriter(writer);
            writer.Flush();
            MediaPlayer.FreeNode(node);
            var vp = sw.ToString();
            Debug.WriteLine(vp);
            SetCurrentValue(VideoParamsProperty, vp);
        }
    }
}
