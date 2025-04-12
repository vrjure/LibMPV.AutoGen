using System;

namespace LibMPVSharp
{
    public class MPVMediaPlayerOptions
    {
        public MPVMediaPlayerOptions()
        {
        }

        public MPVMediaPlayerOptions(string sharePlayerName, MPVMediaPlayer sharedPlayer, bool isWeakReference = false)
        {
            SharePlayerName = sharePlayerName;
            SharedPlayer = sharedPlayer;
            IsWeakReference = isWeakReference;
        }

        public MpvOpenglInitParams_get_proc_addressCallback? GetProcAddress { get; set; }
        public MpvRenderUpdateFn? UpdateCallback { get; set; }
        public Action<MPVMediaPlayer>? BeforeInitialize { get; set; }
        public bool IsWeakReference { get; }
        public MPVMediaPlayer? SharedPlayer { get; }
        public string? SharePlayerName { get; }
    }
}
