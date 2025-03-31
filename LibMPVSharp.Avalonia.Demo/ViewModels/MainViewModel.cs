using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace LibMPVSharp.Avalonia.Demo.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        MediaPlayer = new MPVMediaPlayer();
        MediaPlayer2 = new MPVMediaPlayer(new MPVMediaPlayerOptions("play2", MediaPlayer));
        MediaPlayer3 = new MPVMediaPlayer();
        MediaPlayer4 = new MPVMediaPlayer(new MPVMediaPlayerOptions("player4", MediaPlayer3, true));
    }

    [ObservableProperty]
    private MPVMediaPlayer _mediaPlayer;
    [ObservableProperty]
    private MPVMediaPlayer _mediaPlayer2;
    [ObservableProperty]
    private MPVMediaPlayer _mediaPlayer3;
    [ObservableProperty]
    private MPVMediaPlayer _mediaPlayer4;
}