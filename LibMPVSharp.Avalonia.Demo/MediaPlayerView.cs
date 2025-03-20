using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Labs.Input;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using LibMPVSharp.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibMPVSharp.Avalonia.Demo
{
    public class MediaPlayerView : TemplatedControl
    {
        public static readonly StyledProperty<MPVMediaPlayer?> MediaPlayerProperty = AvaloniaProperty.Register<MediaPlayerView, MPVMediaPlayer?>(nameof(MediaPlayer));
        public MPVMediaPlayer? MediaPlayer
        {
            get => GetValue(MediaPlayerProperty);
            set => SetValue(MediaPlayerProperty, value);
        }

        public static readonly StyledProperty<TimeSpan> DurationProperty = AvaloniaProperty.Register<MediaPlayerView, TimeSpan>(nameof(Duration));
        public TimeSpan Duration
        {
            get => GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        public static readonly StyledProperty<TimeSpan> TimeProperty = AvaloniaProperty.Register<MediaPlayerView, TimeSpan>(nameof(Time));
        public TimeSpan Time
        {
            get => GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        public static readonly StyledProperty<long> VolumeProperty = AvaloniaProperty.Register<MediaPlayerView, long>(nameof(Volume));
        public long Volume
        {
            get => GetValue(VolumeProperty);
            set => SetValue(VolumeProperty, value);
        }

        public static readonly StyledProperty<long> MaxVolumeProperty = AvaloniaProperty.Register<MediaPlayerView, long>(nameof(MaxVolume), 1000L);
        public long MaxVolume
        {
            get => GetValue(MaxVolumeProperty);
            set => SetValue(MaxVolumeProperty, value);
        }

        public static readonly StyledProperty<double> SpeedProperty = AvaloniaProperty.Register<MediaPlayerView, double>(nameof(Speed), 1d);
        public double Speed
        {
            get => GetValue(SpeedProperty);
            set => SetValue(SpeedProperty, value);
        }

        public static readonly StyledProperty<bool> PlayingProperty = AvaloniaProperty.Register<MediaPlayerView, bool>(nameof(Playing), false);
        public bool Playing
        {
            get => (bool)GetValue(PlayingProperty);
            set => SetValue(PlayingProperty, value);
        }

        public static readonly StyledProperty<string?> TitleProperty = AvaloniaProperty.Register<MediaPlayerView, string?>(nameof(Title), "");
        public string? Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly StyledProperty<string> AspectRatioProperty = AvaloniaProperty.Register<MediaPlayerView, string>(nameof(AspectRatio), "no");
        public string AspectRatio
        {
            get => GetValue(AspectRatioProperty);
            set => SetValue(AspectRatioProperty, value);
        }

        public static readonly StyledProperty<string> VideoParamsProperty = AvaloniaProperty.Register<MediaPlayerView, string>(nameof(VideoParams), "");
        public string VideoParams
        {
            get => GetValue(VideoParamsProperty);
            set => SetValue(VideoParamsProperty, value);
        }

        public static readonly RoutedCommand PlayPauseCmd = new RoutedCommand(nameof(PlayPauseCmd));
        public static readonly RoutedCommand OpenFileCmd = new RoutedCommand(nameof(OpenFileCmd));
        public static readonly RoutedCommand SpeedCmd = new RoutedCommand(nameof(SpeedCmd));
        public static readonly RoutedCommand AspectRatioCmd = new RoutedCommand(nameof(AspectRatioCmd));

        private static Queue<string> _aspectRatio = new Queue<string>();
        static MediaPlayerView()
        {
            MediaPlayerProperty.Changed.AddClassHandler<MediaPlayerView>((s, e) => s.OnPropertyChanged(e));
            TimeProperty.Changed.AddClassHandler<MediaPlayerView>((s, e) => s.OnPropertyChanged(e));
            VolumeProperty.Changed.AddClassHandler<MediaPlayerView>((s, e) => s.OnPropertyChanged(e));
            AspectRatioProperty.Changed.AddClassHandler<MediaPlayerView>((s, e) => s.OnPropertyChanged(e));

            _aspectRatio.Enqueue("no");
            _aspectRatio.Enqueue("16:9");
            _aspectRatio.Enqueue("4:3");
        }

        protected override Type StyleKeyOverride => typeof(MediaPlayerView);

        public MediaPlayerView()
        {
            var binds = new[]
            {
                new CommandBinding(PlayPauseCmd, (s,e) => TryPlayPause()),
                new CommandBinding(OpenFileCmd, async (s,e) => await TryOpenFile()),
                new CommandBinding(SpeedCmd, (s,e) => TrySwitchSpeed()),
                new CommandBinding(AspectRatioCmd, (s, e) => TrySwitchAspectRatio())
            };
            CommandManager.SetCommandBindings(this, binds);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            var slider = e.NameScope.Get<Slider>("PART_TimeBar");
            slider.Focus();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == MediaPlayerProperty)
            {
                var oldNew = change.GetOldAndNewValue<MPVMediaPlayer>();

                if (oldNew.oldValue != null)
                {
                    oldNew.oldValue.MpvEvent -= MpvEvent;
                }

                if (oldNew.newValue != null)
                {
                    var player = oldNew.newValue;
                    player.MpvEvent += MpvEvent;
                    
                    SetCurrentValue(SpeedProperty, player.GetPropertyDouble(MPVMediaPlayer.PlaybackControlOpts.Speed));
                    SetCurrentValue(VolumeProperty, player.GetPropertyLong(MPVMediaPlayer.AudioOpts.Volume));
                    SetCurrentValue(MaxVolumeProperty, player.GetPropertyLong(MPVMediaPlayer.AudioOpts.VolumeMax));

                }
            }
            else if (change.Property == TimeProperty)
            {
                if (MediaPlayer == null) return;
                var oldNew = change.GetOldAndNewValue<TimeSpan>();
                if (Math.Abs(oldNew.newValue.TotalSeconds - oldNew.oldValue.TotalSeconds) > 1)
                {
                    MediaPlayer.SetProperty(MPVMediaPlayer.Properties.TimePos, oldNew.newValue.TotalSeconds);
                }
            }
            else if (change.Property == VolumeProperty)
            {
                if (MediaPlayer == null) return;
                var value = change.GetNewValue<long>();
                if (value != MediaPlayer.GetPropertyLong(MPVMediaPlayer.AudioOpts.Volume))
                {
                    MediaPlayer.SetProperty(MPVMediaPlayer.AudioOpts.Volume, value);
                }
            }
            else if (change.Property == AspectRatioProperty)
            {
                if (MediaPlayer == null) return;
                MediaPlayer.SetProperty(MPVMediaPlayer.VideoOpts.VideoAspectOverride, change.GetNewValue<string>());
            }
        }

        private void MpvEvent(object? sender, MpvEvent mpvEvent)
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
                    MpvPropertyChanged(sender, mpvEvent.ReadData<MpvEventProperty>());
                    break;
                case MpvEventId.MPV_EVENT_QUEUE_OVERFLOW:
                    break;
                case MpvEventId.MPV_EVENT_HOOK:
                    break;
                default:
                    break;
            }
        }

        private void MpvPropertyChanged(object? sender, MpvEventProperty property)
        {
            if (property.name == MPVMediaPlayer.Properties.Duration)
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
            Dispatcher.UIThread.InvokeAsync(TryGetVideoParams);
        }
        
        private void DispatchSetCurrentValue(AvaloniaProperty property, object value)
        {
            Dispatcher.UIThread.InvokeAsync(() => SetCurrentValue(property, value));
        }

        private async Task TryOpenFile()
        {
            if (MediaPlayer == null) return;

            var storageProvider = TopLevel.GetTopLevel(this)?.StorageProvider;
            if (storageProvider == null) return;
            var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Media selector",
                FileTypeFilter =
                [
                    new FilePickerFileType("mp4")
                    {
                        Patterns = ["*.mp4"],
                        AppleUniformTypeIdentifiers = ["public.mpeg-4"],
                        MimeTypes = ["video/mp4"]
                    }
                ],
                AllowMultiple = false
            });

            if (files.Count > 0)
            {
                var file =  files[0];
                var path = App.Instance?.UriResolver?.GetRealPath(file.Path);
                
                MediaPlayer.EnsureRenderContextCreated();
                MediaPlayer.ExecuteCommand("loadfile", path);
                SetCurrentValue(PlayingProperty, true);
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
            DispatchSetCurrentValue(VideoParamsProperty, vp);
        }
    }
}
