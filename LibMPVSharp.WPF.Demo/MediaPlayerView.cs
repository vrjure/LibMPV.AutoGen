using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static readonly DependencyProperty MaxVolumeProperty = DependencyProperty.Register(nameof(MaxVolume), typeof(long), typeof(MediaPlayerView), new FrameworkPropertyMetadata(MPVMediaPlayer.DefaultVolumeValue));
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
                    oldValue.MPVPropertyChanged -= view.MPVPropertyChanged;
                }

                if (newVlaue != null)
                {
                    newVlaue.MPVPropertyChanged += view.MPVPropertyChanged;
                    view.SetCurrentValue(VolumeProperty, newVlaue.Volume);
                    view.SetCurrentValue(MaxVolumeProperty, newVlaue.VolumeMax);
                    view.SetCurrentValue(SpeedProperty, newVlaue.Speed);
                }
            }
            else if (e.Property == TimeProperty)
            {
                if (view.MediaPlayer == null) return;
                var oldValue = (TimeSpan)e.OldValue;
                var newValue = (TimeSpan)e.NewValue;
                if (Math.Abs(newValue.TotalSeconds -  oldValue.TotalSeconds) > 1)
                {
                    view.MediaPlayer.TimePos = newValue.TotalSeconds;
                }
            }
            else if (e.Property == VolumeProperty)
            {
                if (view.MediaPlayer == null) return;
                var value = (long)e.NewValue;
                if (value != view.MediaPlayer.Volume)
                {
                    view.MediaPlayer.Volume = value;
                }
            }
            else if (e.Property == AspectRatioProperty)
            {
                if (view.MediaPlayer == null) return;
                view.MediaPlayer.VideoAspectOverride = (string)e.NewValue;
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

        private void MPVPropertyChanged(ref MpvEventProperty property)
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
                    MediaPlayer.Open(file);
                }
                finally
                {
                    Playing = !MediaPlayer.Pause;
                }
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
