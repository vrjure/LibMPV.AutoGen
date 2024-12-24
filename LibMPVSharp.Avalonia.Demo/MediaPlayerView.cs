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
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static readonly StyledProperty<long> MaxVolumeProperty = AvaloniaProperty.Register<MediaPlayerView, long>(nameof(MaxVolume), MPVMediaPlayer.MaxVolumeValue);
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

        public static RoutedCommand PlayPauseCmd { get; private set; }
        public static RoutedCommand OpenFileCmd { get; private set; }
        public static RoutedCommand SpeedCmd { get; private set; }
        public static RoutedCommand AspectRatioCmd { get; private set; }

        private static Queue<string> _aspectRatio = new Queue<string>();
        static MediaPlayerView()
        {
            MediaPlayerProperty.Changed.AddClassHandler<MediaPlayerView>((s, e) => s.OnPropertyChanged(e));
            TimeProperty.Changed.AddClassHandler<MediaPlayerView>((s, e) => s.OnPropertyChanged(e));
            VolumeProperty.Changed.AddClassHandler<MediaPlayerView>((s, e) => s.OnPropertyChanged(e));
            AspectRatioProperty.Changed.AddClassHandler<MediaPlayerView>((s, e) => s.OnPropertyChanged(e));

            PlayPauseCmd = new RoutedCommand(nameof(PlayPauseCmd));
            OpenFileCmd = new RoutedCommand(nameof(OpenFileCmd));
            SpeedCmd = new RoutedCommand(nameof(SpeedCmd));
            AspectRatioCmd = new RoutedCommand(nameof(AspectRatioCmd));

            _aspectRatio.Enqueue("no");
            _aspectRatio.Enqueue("16:9");
            _aspectRatio.Enqueue("4:3");
        }

        protected override Type StyleKeyOverride => typeof(MediaPlayerView);

        public MediaPlayerView()
        {
            var binds = new[]
            {
                new CommandBinding(PlayPauseCmd, (s,e) => TryPlayPause(), (s, e) => e.CanExecute = true),
                new CommandBinding(OpenFileCmd, async (s,e) => await TryOpenFile(), (s, e) => e.CanExecute = true),
                new CommandBinding(SpeedCmd, (s,e) => TrySwitchSpeed(), (s, e) => e.CanExecute = true),
                new CommandBinding(AspectRatioCmd, (s, e) => TrySwitchAspectRatio(), (s, e) => e.CanExecute = true)
            };
            CommandManager.SetCommandBindings(this, binds);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == MediaPlayerProperty)
            {
                var oldNew = change.GetOldAndNewValue<MPVMediaPlayer>();

                if (oldNew.oldValue != null)
                {
                    oldNew.oldValue.MPVPropertyChanged -= OldValue_MPVPropertyChanged;
                }

                if (oldNew.newValue != null)
                {
                    var player = oldNew.newValue;
                    player.MPVPropertyChanged += OldValue_MPVPropertyChanged;
                    SetCurrentValue(SpeedProperty, player.Speed);
                    SetCurrentValue(VolumeProperty, player.Volume);
                    SetCurrentValue(MaxVolumeProperty, player.VolumeMax);

                }
            }
            else if (change.Property == TimeProperty)
            {
                if (MediaPlayer == null) return;
                var oldNew = change.GetOldAndNewValue<TimeSpan>();
                if (Math.Abs(oldNew.newValue.TotalSeconds - oldNew.oldValue.TotalSeconds) > 1)
                {
                    MediaPlayer.TimePos = oldNew.newValue.TotalSeconds;
                }
            }
            else if (change.Property == VolumeProperty)
            {
                if (MediaPlayer == null) return;
                var value = change.GetNewValue<long>();
                if (value != MediaPlayer.Volume)
                {
                    MediaPlayer.Volume = value;
                }
            }
            else if (change.Property == AspectRatioProperty)
            {
                if (MediaPlayer == null) return;
                MediaPlayer.VideoAspectOverride = change.GetNewValue<string>();
            }
        }

        private void OldValue_MPVPropertyChanged(ref MpvEventProperty property)
        {
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
                        MimeTypes = ["application/mp4"]
                    }
                ],
                AllowMultiple = false
            });

            if (files.Count > 0)
            {
                MediaPlayer.Open(files[0].TryGetLocalPath()!);
                SetCurrentValue(PlayingProperty, true);
            }
        }

        private void TryPlayPause()
        {
            if (MediaPlayer == null) return;
            MediaPlayer.Pause = !MediaPlayer.Pause;
        }

        private void TrySwitchSpeed()
        {
            if (MediaPlayer == null) return;

            var speed = MediaPlayer.Speed;
            speed++;
            if (speed > 2)
            {
                MediaPlayer.Speed = 1;
            }
            else
            {
                MediaPlayer.Speed = speed;
            }
        }

        private void TrySwitchAspectRatio()
        {
            if (MediaPlayer == null) return;
            var ratio = _aspectRatio.Dequeue();
            _aspectRatio.Enqueue(ratio);
            AspectRatio = ratio;
        }
    }
}
