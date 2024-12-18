using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace LibMPVSharp.WPF.Demo
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            _mediaPlayer = new MPVMediaPlayer();
        }

        [ObservableProperty]
        private MPVMediaPlayer _mediaPlayer;


        [RelayCommand]
        private void PlayPause(string uri)
        {
            if (MediaPlayer == null || string.IsNullOrEmpty(uri)) return;

            var path = MediaPlayer.Path;
            if (string.IsNullOrEmpty(path) || path != uri)
            {
                MediaPlayer.Stop();
                MediaPlayer.Open(uri);
            }
            else
            {
                MediaPlayer.Pause = !MediaPlayer.Pause;
            }
        }


        [RelayCommand]
        private void Stop()
        {
            MediaPlayer?.Stop();
        }
    }
}
