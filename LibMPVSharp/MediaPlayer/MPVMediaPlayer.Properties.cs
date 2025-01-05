using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace LibMPVSharp
{
    public partial class MPVMediaPlayer
    {
        public static readonly double SpeedMinValue = 0.01d;
        public static readonly double SpeedMaxValue = 100;
        public static readonly long MaxVolumeValue = 1000;
        public static readonly long DefaultVolumeValue = 130;

        #region property
        public string? FileName
        {
            get => GetPropertyString("filename");
        }

        public string? Path
        {
            get => GetPropertyString("path");
        }

        public double Duration
        {
            get => GetPropertyDouble("duration");
        }

        public bool DisplaySyncActive
        {
            get => GetPropertyString("video-sync") == "display";
        }

        public long PercentPos
        {
            get => GetPropertyLong("percent-pos");
            set => SetProperty("percent-pos", value);
        }

        public double TimePos
        {
            get => GetPropertyDouble("time-pos");
            set => SetProperty("time-pos", value);
        }

        public bool Seeking
        {
            get => GetPropertyBoolean("seeking");
        }

        public long Width
        {
            get => GetPropertyLong("width");
        }

        public long Height
        {
            get => GetPropertyLong("height");
        }

        public long DWidth
        {                  
            get => GetPropertyLong("dwidth");
        }

        public long DHeight
        {
            get => GetPropertyLong("dheight");
        }

        public string? HwDec
        {
            get => GetPropertyString("hwdec");
            set => SetProperty("hwdec", value);
        }

        public string? MPVVersion
        {
            get => GetPropertyString("mpv-version");
        }

        public string? FFMpegVersion
        {
            get => GetPropertyString("ffmpeg-version");
        }

        public long LibASSVersion
        {
            get => GetPropertyLong("libass-version");
        }

        public string? Platform
        {
            get => GetPropertyString("platform");
        }
        #endregion

        #region track selection

        public string? ALang
        {
            get => GetPropertyString("alang");
            set => SetProperty("alang", value);
        }

        public string? SLang
        {
            get => GetPropertyString("slang");
            set => SetProperty("slang", value);
        }

        public string? VLang
        {
            get => GetPropertyString("vlang");
            set => SetProperty("vlang", value);
        }

        public string? AId
        {
            get => GetPropertyString("aid");
            set => SetProperty("aid", value);
        }

        public string? SId
        {
            get => GetPropertyString("sid");
            set => SetProperty("sid", value);
        }

        public string? VId
        {
            get => GetPropertyString("vid");
            set => SetProperty("vid", value);
        }

        public string? Edition
        {
            get => GetPropertyString("edition");
            set => SetProperty("edition", value);
        }

        public bool TrackAutoSelection
        {
            get => GetPropertyBoolean("track-auto-selection");
            set => SetProperty("track-auto-selection", value);
        }

        public string? SubsWithMatchingAudio
        {
            get => GetPropertyString("subs-with-matching-audio");
            set => SetProperty("subs-with-matching-audio", value);
        }

        public bool SubsMatchOsLanguage
        {
            get => GetPropertyBoolean("subs-match-os-language");
            set => SetProperty("subs-match-os-language", value);
        }

        public string? SubsFallback
        {
            get => GetPropertyString("subs-fallback");
            set => SetProperty("subs-fallback", value);
        }

        public string? SubsFallbackForced
        {
            get => GetPropertyString("subs-fallback-forced");
            set => SetProperty("subs-fallback-forced", value);
        }

        #endregion

        #region playback control

        public string? Start
        {
            get => GetPropertyString("start");
            set => SetProperty("start", value);
        }

        public string? End
        {
            get => GetPropertyString("end");
            set => SetProperty("end", value);
        }

        public string? Length
        {
            get => GetPropertyString("length");
            set => SetProperty("length", value);
        }

        public bool RebaseStartTime
        {
            get => GetPropertyBoolean("rebase-start-time");
            set => SetProperty("rebase-start-time", value);
        }

        public double Speed
        {
            get => GetPropertyDouble("speed");
            set => SetProperty("speed", value);
        }

        public double Pitch
        {
            get => GetPropertyDouble("pitch");
            set => SetProperty("pitch", value);
        }

        public bool Pause
        {
            get => GetPropertyBoolean("pause");
            set => SetProperty("pause", value);
        }

        public bool Shuffle
        {
            get => GetPropertyBoolean("shuffle");
            set => SetProperty("shuffle", value);
        }

        public string? PlaylistStart
        {
            get => GetPropertyString("playlist-start");
            set => SetProperty("playlist-start", value);
        }

        public string? Playlist
        {
            get => GetPropertyString("playlist");
            set => SetProperty("playlist", value);
        }

        public long ChapterMergeThreshold
        {
            get => GetPropertyLong("chapter-merge-threshold");
            set => SetProperty("chapter-merge-threshold", value);
        }

        public double ChapterSeekThreshold
        {
            get => GetPropertyDouble("chapter-seek-threshold");
            set => SetProperty("chatper-seek-threshold", value);
        }

        public string? HrSeek
        {
            get => GetPropertyString("hr-seek");
            set => SetProperty("hr-seek", value);
        }

        public double HrSeekDemuxerOffset
        {
            get => GetPropertyDouble("hr-seek-demuxer-offset");
            set => SetProperty("hr-seek-demuxer-offset", value);
        }

        public bool HrSeekFramedrop
        {
            get => GetPropertyBoolean("hr-seek-framedrop");
            set => SetProperty("hr-seek=framedrop", value);
        }

        public bool LoadUnsafePlaylists
        {
            get => GetPropertyBoolean("load-unsafe-playlists");
            set => SetProperty("load-unsafe-playlists", value);
        }

        public bool AccessReferences
        {
            get => GetPropertyBoolean("access-references");
            set => SetProperty("access-references", value);
        }

        public string? LoopPlaylist
        {
            get => GetPropertyString("loop-playlist");
            set => SetProperty("loop-playlist", value);
        }

        public string? LoopFile
        {
            get => GetPropertyString("loop-file");
            set => SetProperty("loop-file", value);
        }

        public long ABLoopA
        {
            get => GetPropertyLong("ab-loop-a");
            set => SetProperty("ab-loop-b", value);
        }

        public long ABLoopB
        {
            get => GetPropertyLong("ab-loop-b");
            set => SetProperty("ab-loop-b", value);
        }

        public string? ABLoopCount
        {
            get => GetPropertyString("ab-loop-count");
            set => SetProperty("ab-loop-count", value);
        }

        public bool OrderedChapters
        {
            get => GetPropertyBoolean("ordered-chapters");
            set => SetProperty("ordered-chapters", value);
        }

        public string? OrderedChaptersFiles
        {
            get => GetPropertyString("ordered-chapters-files");
            set => SetProperty("ordered-chapters-files", value);
        }

        public string? ChaptersFile
        {
            get => GetPropertyString("chapters-file");
            set => SetProperty("chapters-file", value);
        }

        public double SStep
        {
            get => GetPropertyDouble("sstep");
            set => SetProperty("sstep", value);
        }

        public bool StopPlaybackOnInitFailure
        {
            get => GetPropertyBoolean("stop-playback-on-init-failure");
            set => SetProperty("stop-playback-init-failure", value);
        }

        public string? PlayDirection
        {
            get => GetPropertyString("play-direction");
            set => SetProperty("play-direction", value);
        }

        public long VideoReversalBuffer
        {
            get => GetPropertyLong("video-reversal-buffer");
            set => SetProperty("video-reversal-buffer", value);
        }

        public long AudioReversalBuffer
        {
            get => GetPropertyLong("audio-reversal-buffer");
            set => SetProperty("audio-reversal-buffer", value);
        }

        public string? VideoBackwardOverlap
        {
            get => GetPropertyString("video-backward-overlap");
            set => SetProperty("video-backward-overlap", value);
        }

        public string? AudioBackwardOverlap
        {
            get => GetPropertyString("audio-backward-overlap");
            set => SetProperty("audio-backward-overlap", value);
        }

        public long VideoBackwardBatch
        {
            get => GetPropertyLong("video-backward-batch");
            set => SetProperty("video-backward-batch", value);
        }

        public long AudioBackwardBatch
        {
            get => GetPropertyLong("audio-backward-batch");
            set => SetProperty("audio-backward-batch", value);
        }

        public long DemuxerBackwardPlaybackStep
        {
            get => GetPropertyLong("demuxer-backward-playback-step");
            set => SetProperty("demuxer-backward-playback-stop", value);
        }
        #endregion

        #region program behavior

        public string? LogFile
        {
            get => GetPropertyString("log-file");
            set => SetProperty("log-file", value);
        }

        public string? ConfigDir
        {
            get => GetPropertyString("config-dir");
            set => SetProperty("config-dir", value);
        }

        public string? DumpStats
        {
            get => GetPropertyString("dump-stats");
            set => SetProperty("dump-stats", value);
        }

        public string? Idle
        {
            get => GetPropertyString("idle");
            set => SetProperty("idle", value);
        }

        public string? Include
        {
            get => GetPropertyString("include");
            set => SetProperty("include", value);
        }

        public bool LoadScripts
        {
            get => GetPropertyBoolean("load-scripts");
            set => SetProperty("load-scripts", value);
        }

        public string? Script
        {
            get => GetPropertyString("script");
            set => SetProperty("script", value);
        }

        public string? Scripts
        {
            get => GetPropertyString("scripts");
            set => SetProperty("scripts", value);
        }

        public string? ScriptOpt
        {
            get => GetPropertyString("script-opt");
            set => SetProperty("script-opt", value);
        }

        public string? ScriptOpts
        {
            get => GetPropertyString("script-opts");
            set => SetProperty("script-opts", value);
        }

        public string? Profile
        {
            get => GetPropertyString("profile");
            set => SetProperty("profile", value);
        }

        public string? ResetOnNextFile
        {
            get => GetPropertyString("reset-on-next-file");
            set => SetProperty("reset-on-next-file", value);
        }

        public bool Ytdl
        {
            get => GetPropertyBoolean("ytdl");
            set => SetProperty("ytdl", value);
        }

        public string? YtdlFormat
        {
            get => GetPropertyString("ytdl-format");
            set => SetProperty("ytdl-format", value);
        }

        public string? YtdlRawOptions
        {
            get => GetPropertyString("ytdl-raw-options");
            set => SetProperty("ytdl-raw-options", value);
        }

        public bool JsMemoryReport
        {
            get => GetPropertyBoolean("js-memory-report");
            set => SetProperty("js-memory-report", value);
        }

        public bool LoadStatsOverlay
        {
            get => GetPropertyBoolean("load-stats-overlay");
            set => SetProperty("load-stats-overlay", value);
        }

        public string? LoadAutoProfiles
        {
            get => GetPropertyString("load-auto-profiles");
            set => SetProperty("load-auto-profiles", value);
        }

        public bool LoadSelect
        {
            get => GetPropertyBoolean("load-select");
            set => SetProperty("load-select", value);
        }

        public string? PlayerOperationMode
        {
            get => GetPropertyString("player-operation-mode");
            set => SetProperty("player-operation-mode", value);
        }

        #endregion

        #region Video
        public string? VideoAspectOverride
        {
            get => GetPropertyString("video-aspect-override");
            set => SetProperty("video-aspect-override", value);
        }

        public string? VideoUnscaled
        {
            get => GetPropertyString("video-unscaled");
            set => SetProperty("video-unscaled", value);
        }

        public long VideoZoom
        {
            get => GetPropertyLong("video-zoom");
            set => SetProperty("video-zoom", value);
        }

        public double VideoScaleX
        {
            get => GetPropertyDouble("video-scale-x");
            set => SetProperty("video-scale-x", value);
        }

        public double VideoScaleY
        {
            get => GetPropertyDouble("video-scale-y");
            set => SetProperty("video-scale-y", value);
        }

        public double VideoAlignX
        {
            get => GetPropertyDouble("video-align-x");
            set => SetProperty("video-align-x", value);
        }

        public double VideoAlignY
        {
            get => GetPropertyDouble("video-align-y");
            set => SetProperty("video-align-y", value);
        }

        public double VideoMarginRatioLeft
        {
            get => GetPropertyDouble("video-margin-ratio-left");
            set => SetProperty("video-margin-ratio-left", value);
        }

        public double VideoMarginRatioRight
        {
            get => GetPropertyDouble("video-margin-ratio-right");
            set => SetProperty("video-margin-ratio-right", value);
        }

        public double VideoMarginRatioTop
        {
            get => GetPropertyDouble("video-margin-ratio-top");
            set => SetProperty("video-margin-ratio-top", value);
        }

        public double VideoMarginRatioBottom
        {
            get => GetPropertyDouble("video-margin-ratio-bottom");
            set => SetProperty("video-margin-ratio-bottom", value);
        }

        #endregion

        #region Audio
        public long Volume
        {
            get => GetPropertyLong("volume");
            set => SetProperty("volume", value);
        }

        public long VolumeMax
        {
            get => GetPropertyLong("volume-max");
            set => SetProperty("volume-max", value);
        }

        public bool Mute
        {
            get => GetPropertyBoolean("mute");
            set => SetProperty("mute", value);
        }
        #endregion

        #region screen shot
        public string? ScreenshotFormat
        {
            get => GetPropertyString("screenshot-format");
            set => SetProperty("screenshot-format", value);
        }

        public string? ScreenshotTemplate
        {
            get => GetPropertyString("screenshot-template");
            set => SetProperty("screenshot-template", value);
        }

        public string? ScreenshotDir
        {
            get => GetPropertyString("screenshot-dir");
            set => SetProperty("screenshot-dir", value);
        }

        public long ScreenshotJpegQuality
        {
            get => GetPropertyLong("screenshot-jpeg-quality");
            set => SetProperty("screenshot-jpeg-quality", value);
        }

        public long ScreenshotPngCompression
        {
            get => GetPropertyLong("screenshot-png-compression");
            set => SetProperty("screenshot-png-compression", value);
        }

        public long ScreenshotPngFilter
        {
            get => GetPropertyLong("screenshot-png-filter");
            set => SetProperty("screenshot-png-filter", value);
        }

        public bool ScreenshotSW
        {
            get => GetPropertyBoolean("screenshot-sw");
            set => SetProperty("screenshot-sw", value);
        }
        #endregion

        #region terminal
        public string? Cache
        {
            get => GetPropertyString("cache");
            set => SetProperty("cache", value);
        }

        public long CacheSecs
        {
            get => GetPropertyLong("cache-secs");
            set => SetProperty("cache-secs", value);
        }

        public bool CacheOnDisk
        {
            get => GetPropertyBoolean("cache-on-disk");
            set => SetProperty("cache-on-disk", value);
        }

        public string? DemuxerCacheDir
        {
            get => GetPropertyString("demuxer-cache-dir");
            set => SetProperty("demuxer-cache-dir", value);
        }

        public bool CachePause
        {
            get => GetPropertyBoolean("cache-pause");
            set => SetProperty("cache-pause", value);
        }

        public long CachePauseWait
        {
            get => GetPropertyLong("cache-pause-wait");
            set => SetProperty("cache-pause-wait", value);
        }

        public bool CachePauseInitial
        {
            get => GetPropertyBoolean("cache-pause-initial");
            set => SetProperty("cache-pause-initial", value);
        }
        #endregion


        #region network
        public long StreamBufferSize
        {
            get => GetPropertyLong("stream-buffer-size");
            set => SetProperty("stream-buffer-size", value);
        }

        public string? UserAgent
        {
            get => GetPropertyString("user-agent");
            set => SetProperty("user-agent", value);
        }

        public bool Cookies
        {
            get => GetPropertyBoolean("cookies");
            set => SetProperty("cookies", value);
        }

        public string? CookiesFile
        {
            get => GetPropertyString("cookies-file");
            set => SetProperty("cookies-file", value);
        }

        public string? HttpHeaderFields
        {
            get => GetPropertyString("http-header-fields");
            set => SetProperty("http-header-fields", value);
        }

        public string? HttpProxy
        {
            get => GetPropertyString("http-proxy");
            set => SetProperty("http-proxy", value);
        }

        public long NetworkTimeout
        {
            get => GetPropertyLong("network-timeout");
            set => SetProperty("network-timeout", value);
        }
        #endregion

        #region OSD
        public long OSDDimensionsW
        {
            get => GetPropertyLong("osd-dimensions/w");
        }
        #endregion
    }
}
