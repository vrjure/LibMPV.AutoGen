using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace LibMPVSharp.Avalonia.Demo.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        _mediaPlayer = new MPVMediaPlayer();
    }

    [ObservableProperty]
    private MPVMediaPlayer _mediaPlayer;
}