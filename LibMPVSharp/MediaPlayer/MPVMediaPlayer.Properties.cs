using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace LibMPVSharp
{
    /// <summary>
    /// <see cref="https://mpv.io/manual/master/#options"/>
    /// </summary>
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

        #region watch later

        public string? WatchLaterDir
        {
            get => GetPropertyString("watch-later-dir");
            set => SetProperty("watch-later-dir", value);
        }

        public bool ResumePlayback
        {
            get => GetPropertyBoolean("resume-playback");
            set => SetProperty("resume-playback", value);
        }

        public bool ResumePlaybackCheckMtime
        {
            get => GetPropertyBoolean("resume-playback-check-mtime");
            set => SetProperty("resume-playback-check-mtime", value);
        }

        public string? WatchLaterOptions
        {
            get => GetPropertyString("watch-later-options");
            set => SetProperty("watch-later-options", value);
        }
        #endregion

        #region video
        public string? VO
        {
            get => GetPropertyString("vo");
            set => SetProperty("vo", value);
        }

        public string? VD
        {
            get => GetPropertyString("vd");
            set => SetProperty("vd", value);
        }

        public string? VF
        {
            get => GetPropertyString("vf");
            set => SetProperty("vf", value);
        }

        public string? FrameDrop
        {
            get => GetPropertyString("frame-drop");
            set => SetProperty("frame-drop", value);
        }

        public bool VideoLatencyHacks
        {
            get => GetPropertyBoolean("video-latency-hacks");
            set => SetProperty("video-latency-hacks", value);
        }

        public string? Hwdec
        {
            get => GetPropertyString("hwdec");
            set => SetProperty("hwdec", value);
        }

        public string? GpuHwdecInterop
        {
            get => GetPropertyString("gpu-hwdec-interop");
            set => SetProperty("gpu-hwdec-interop", value);
        }

        public long HwdecExtraFrames
        {
            get => GetPropertyLong("hwdec-extra-frames");
            set => SetProperty("hwdec-extra-frames", value);
        }

        public string? HwdecImageFormat
        {
            get => GetPropertyString("hwdec-image-format");
            set => SetProperty("hwdec-image-format", value);
        }

        public string? CudaDecodeDevice
        {
            get => GetPropertyString("cuda-decode-device");
            set => SetProperty("cuda-decode-device", value);
        }

        public string? VaapiDevice
        {
            get => GetPropertyString("vaapi-device");
            set => SetProperty("vaapi-device", value);
        }

        public double Panscan
        {
            get => GetPropertyDouble("panscan");
            set => SetProperty("panscan", value);
        }

        public string? VideoAspectOverride
        {
            get => GetPropertyString("video-aspect-override");
            set => SetProperty("video-aspect-override", value);
        }

        public string? VideoAspectMethod
        {
            get => GetPropertyString("video-aspect-method");
            set => SetProperty("video-aspect-method", value);
        }

        public string? VideoUnscaled
        {
            get => GetPropertyString("video-unscaled");
            set => SetProperty("video-unscaled", value);
        }

        public double VideoPanX
        {
            get => GetPropertyDouble("video-pan-x");
            set => SetProperty("video-pan-x", value);
        }

        public double VideoPanY
        {
            get => GetPropertyDouble("video-pan-y");
            set => SetProperty("video-pan-y", value);
        }

        public long VideoRotate
        {
            get => GetPropertyLong("video-rotate");
            set => SetProperty("video-rotate", value);
        }

        public string? VideoCrop
        {
            get => GetPropertyString("video-crop");
            set => SetProperty("video-crop", value);
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

        public bool VideoRecenter
        {
            get => GetPropertyBoolean("video-recenter");
            set => SetProperty("video-recenter", value);
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

        public bool CorrectPts
        {
            get => GetPropertyBoolean("correct-pts");
            set => SetProperty("correct-pts", value);
        }

        public string? Deinterlace
        {
            get => GetPropertyString("deinterlace");
            set => SetProperty("deinterlace", value);
        }

        public string? DeinterlaceFieldParity
        {
            get => GetPropertyString("deinterlace-field-parity");
            set => SetProperty("deinterlace-field-parity", value);
        }

        public string? VideoOutputLevels
        {
            get => GetPropertyString("video-output-levels");
            set => SetProperty("video-output-levels", value);
        }

        public string? HwdecCodecs
        {
            get => GetPropertyString("hwdec-codecs");
            set => SetProperty("hwdec-codecs", value);
        }

        public bool VdLavcCheckHwProfile
        {
            get => GetPropertyBoolean("vd-lavc-check-hw-profile");
            set => SetProperty("vd-lavc-check-hw-profile", value);
        }

        public bool VdLavcSoftwareFallback
        {
            get => GetPropertyBoolean("vd-lavc-software-fallback");
            set => SetProperty("vd-lavc-software-fallback", value);
        }

        public string? VdLavcFilmGrain
        {
            get => GetPropertyString("vd-lavc-film-grain");
            set => SetProperty("vd-lavc-film-grain", value);
        }

        public string? VdLavcDr
        {
            get => GetPropertyString("vd-lavc-dr");
            set => SetProperty("vd-lavc-dr", value);
        }

        public string? VdLavcO
        {
            get => GetPropertyString("vd-lavc-o");
            set => SetProperty("vd-lavc-o", value);
        }

        public bool VdLavcShowAll
        {
            get => GetPropertyBoolean("vd-lavc-show-all");
            set => SetProperty("vd-lavc-show-all", value);
        }

        public string? VdLavcSkiploopfilter
        {
            get => GetPropertyString("vd-lavc-skiploopfilter");
            set => SetProperty("vd-lavc-skiploopfilter", value);
        }

        public string? VdLavcSkipidct
        {
            get => GetPropertyString("vd-lavc-skip-idct");
            set => SetProperty("vd-lavc-skip-idct", value);
        }

        public string? VdLavcSkipframe
        {
            get => GetPropertyString("vd-lavc-skipframe");
            set => SetProperty("vd-lavc-skipframe", value);
        }

        public string? VdLavcFramedrop
        {
            get => GetPropertyString("vd-lavc-framedrop");
            set => SetProperty("vd-lavc-framedrop", value);
        }

        public long VdLavcThreads
        {
            get => GetPropertyLong("vd-lavc-threads");
            set => SetProperty("vd-lavc-threads", value);
        }

        public bool VdLavcAssumeOldX264
        {
            get => GetPropertyBoolean("vd-lavc-assume-old-x264");
            set => SetProperty("vd-lavc-assume-old-x264", value);
        }

        public long SwapchainDepth
        {
            get => GetPropertyLong("swapchain-depth");
            set => SetProperty("swapchain-depth", value);
        }
        #endregion

        #region audio

        public bool AudioPitchCorrection
        {
            get => GetPropertyBoolean("audio-pitch-correction");
            set => SetProperty("audio-pitch-correction", value);
        }

        public string? AudioDevice
        {
            get => GetPropertyString("audio-device");
            set => SetProperty("audio-device", value);
        }

        public bool AudioExclusive
        {
            get => GetPropertyBoolean("audio-exclusive");
            set => SetProperty("audio-exclusive", value);
        }

        public bool AudioFallbackToNull
        {
            get => GetPropertyBoolean("audio-fallback-to-null");
            set => SetProperty("audio-fallback-to-null", value);
        }

        public string? AO
        {
            get => GetPropertyString("ao");
            set => SetProperty("ao", value);
        }

        public string? AF
        {
            get => GetPropertyString("af");
            set => SetProperty("af", value);
        }

        public string? AD
        {
            get => GetPropertyString("ad");
            set => SetProperty("ad", value);
        }

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

        public double VolumeGain
        {
            get => GetPropertyDouble("volume-gain");
            set => SetProperty("volume-gain", value);
        }

        public double VolumeGainMax
        {
            get => GetPropertyDouble("volume-gain-max");
            set => SetProperty("volume-gain-max", value);
        }

        public string? Replaygain
        {
            get => GetPropertyString("replaygain");
            set => SetProperty("replaygain", value);
        }

        public string? ReplaygainPreamp
        {
            get => GetPropertyString("replaygain-preamp");
            set => SetProperty("replaygain-preamp", value);
        }

        public bool ReplaygainClip
        {
            get => GetPropertyBoolean("replaygain-clip");
            set => SetProperty("replaygain-clip", value);
        }

        public double ReplaygainFallback
        {
            get => GetPropertyDouble("replaygain-fallback");
            set => SetProperty("replaygain-fallback", value);
        }

        public double AudioDelay
        {
            get => GetPropertyDouble("audio-delay");
            set => SetProperty("audio-delay", value);
        }

        public bool Mute
        {
            get => GetPropertyBoolean("mute");
            set => SetProperty("mute", value);
        }

        public string? AudioDemuxer
        {
            get => GetPropertyString("audio-demuxer");
            set => SetProperty("audio-demuxer", value);
        }

        public long AdLavcAc3drc
        {
            get => GetPropertyLong("ad-lavc-ac3drc");
            set => SetProperty("ad-lavc-ac3drc", value);
        }

        public bool AdLavcDownmix
        {
            get => GetPropertyBoolean("ad-lavc-downmix");
            set => SetProperty("ad-lavc-downmix", value);
        }

        public long AdLavcThreads
        {
            get => GetPropertyLong("ad-lavc-threads");
            set => SetProperty("ad-lavc-threads", value);
        }

        public string? AdLavcO
        {
            get => GetPropertyString("ad-lavc-o");
            set => SetProperty("ad-lavc-o", value);
        }

        public string? AudioSpdif
        {
            get => GetPropertyString("audio-spdif");
            set => SetProperty("audio-spdif", value);
        }

        public string? AudioChannels
        {
            get => GetPropertyString("audio-channels");
            set => SetProperty("audio-channels", value);
        }

        public string? AudioDisplay
        {
            get => GetPropertyString("audio-display");
            set => SetProperty("audio-display", value);
        }

        public string? AudioFiles
        {
            get => GetPropertyString("audio-files");
            set => SetProperty("audio-files", value);
        }

        public string? AudioFile
        {
            get => GetPropertyString("audio-file");
            set => SetProperty("audio-file", value);
        }

        public string? AudioFormat
        {
            get => GetPropertyString("audio-format");
            set => SetProperty("audio-format", value);
        }

        public double AudioSamplerate
        {
            get => GetPropertyDouble("audio-samplerate");
            set => SetProperty("audio-samplerate", value);
        }

        public string? GaplessAudio
        {
            get => GetPropertyString("gapless-audio");
            set => SetProperty("gapless-audio", value);
        }

        public bool InitialAudioSync
        {
            get => GetPropertyBoolean("initial-audio-sync");
            set => SetProperty("initial-audio-sync", value);
        }

        public string? AudioFileAuto
        {
            get => GetPropertyString("audio-file-auto");
            set => SetProperty("audio-file-auto", value);
        }

        public string? AudioExts
        {
            get => GetPropertyString("audio-exts");
            set => SetProperty("audio-exts", value);
        }

        public string? AudioFilePaths
        {
            get => GetPropertyString("audio-file-paths");
            set => SetProperty("audio-file-paths", value);
        }

        public string? AudioClientName
        {
            get => GetPropertyString("audio-client-name");
            set => SetProperty("audio-client-name", value);
        }

        public double AudioBuffer
        {
            get => GetPropertyDouble("audio-buffer");
            set => SetProperty("audio-buffer", value);
        }

        public bool AudioStreamSilence
        {
            get => GetPropertyBoolean("audio-stream-silence");
            set => SetProperty("audio-stream-silence", value);
        }

        public double AudioWaitOpen
        {
            get => GetPropertyDouble("audio-wait-open");
            set => SetProperty("audio-wait-open", value);
        }
        #endregion

        #region subtitles
        public string? SubDemuxer
        {
            get => GetPropertyString("sub-demuxer");
            set => SetProperty("sub-demuxer", value);
        }

        public string? SubLavcO
        {
            get => GetPropertyString("sub-lavc-o");
            set => SetProperty("sub-lavc-o", value);
        }

        public double SubDelay
        {
            get => GetPropertyDouble("sub-delay");
            set => SetProperty("sub-delay", value);
        }

        public double SecondarySubDelay
        {
            get => GetPropertyDouble("secondary-sub-delay");
            set => SetProperty("secondary-sub-delay", value);
        }

        public string? SubFiles
        {
            get => GetPropertyString("sub-files");
            set => SetProperty("sub-files", value);
        }

        public string? SubFile
        {
            get => GetPropertyString("sub-file");
            set => SetProperty("sub-file", value);
        }

        public string? SecondarySid
        {
            get => GetPropertyString("secondary-sid");
            set => SetProperty("secondary-sid", value);
        }

        public long SubScale
        {
            get => GetPropertyLong("sub-scale");
            set => SetProperty("sub-scale", value);
        }

        public bool SubScaleSigns
        {
            get => GetPropertyBoolean("sub-scale-signs");
            set => SetProperty("sub-scale-signs", value);
        }

        public bool SubScaleByWindow
        {
            get => GetPropertyBoolean("sub-scale-by-window");
            set => SetProperty("sub-scale-by-window", value);
        }

        public bool SubScaleWithWindow
        {
            get => GetPropertyBoolean("sub-scale-with-window");
            set => SetProperty("sub-scale-with-window", value);
        }

        public bool SubAssScaleWithWindow
        {
            get => GetPropertyBoolean("sub-ass-scale-with-window");
            set => SetProperty("sub-ass-scale-with-window", value);
        }

        public bool Embeddedfonts
        {
            get => GetPropertyBoolean("embeddedfonts");
            set => SetProperty("embeddedfonts", value);
        }

        public long SubPos
        {
            get => GetPropertyLong("sub-pos");
            set => SetProperty("sub-pos", value);
        }

        public long SecondarySubPos
        {
            get => GetPropertyLong("secondary-sub-pos");
            set => SetProperty("secondary-sub-pos", value);
        }

        public double SubSpeed
        {
            get => GetPropertyDouble("sub-speed");
            set => SetProperty("sub-speed", value);
        }

        public string? SubAssStyleOverrides
        {
            get => GetPropertyString("sub-ass-style-overrides");
            set => SetProperty("sub-ass-style-overrides", value);
        }

        public string? SubHinting
        {
            get => GetPropertyString("sub-hinting");
            set => SetProperty("sub-hinting", value);
        }

        public double SubLineSpacing
        {
            get => GetPropertyDouble("sub-line-spacing");
            set => SetProperty("sub-line-spacing", value);
        }

        public string? SubShaper
        {
            get => GetPropertyString("sub-shaper");
            set => SetProperty("sub-shaper", value);
        }

        public double SubAssPruneDelay
        {
            get => GetPropertyDouble("sub-ass-prune-delay");
            set => SetProperty("sub-ass-prune-delay", value);
        }

        public string? SubAssStyles
        {
            get => GetPropertyString("sub-ass-styles");
            set => SetProperty("sub-ass-styles", value);
        }

        public string? SubAssOverride
        {
            get => GetPropertyString("sub-ass-override");
            set => SetProperty("sub-ass-override", value);
        }

        public string? SecondarySubAssOverride
        {
            get => GetPropertyString("secondary-sub-ass-override");
            set => SetProperty("secondary-sub-ass-override", value);
        }

        public bool SubAssForceMargins
        {
            get => GetPropertyBoolean("sub-ass-force-margins");
            set => SetProperty("sub-ass-force-margins", value);
        }

        public bool SubUseMargins
        {
            get => GetPropertyBoolean("sub-use-margins");
            set => SetProperty("sub-use-margins", value);
        }

        public string? SubAssUseVideoData
        {
            get => GetPropertyString("sub-ass-use-video-data");
            set => SetProperty("sub-ass-use-video-data", value);
        }

        public string? SubAssVideoAspectOverride
        {
            get => GetPropertyString("sub-ass-use-video-data");
            set => SetProperty("sub-ass-use-video-data", value);
        }

        public bool SubVsfilterBidiCompat
        {
            get => GetPropertyBoolean("sub-vsfilter-bidi-compat");
            set => SetProperty("sub-vsfilter-bidi-compat", value);
        }

        public string? SubAssVsfilterColorCompat
        {
            get => GetPropertyString("sub-ass-vsfilter-color-compat");
            set => SetProperty("sub-ass-vsfilter-color-compat", value);
        }

        public bool StretchDvdSubs
        {
            get => GetPropertyBoolean("stretch-dvd-subs");
            set => SetProperty("stretch-dvd-subs", value);
        }

        public bool StretchImageSubsToScreen
        {
            get => GetPropertyBoolean("stretch-image-subs-to-screen");
            set => SetProperty("stretch-image-subs-to-screen", value);
        }

        public bool ImageSubsVideoResolution
        {
            get => GetPropertyBoolean("image-subs-video-resolution");
            set => SetProperty("image-subs-video-resolution", value);
        }

        public bool SubAss
        {
            get => GetPropertyBoolean("sub-ass");
            set => SetProperty("sub-ass", value);
        }

        public string? SubAuto
        {
            get => GetPropertyString("sub-auto");
            set => SetProperty("sub-auto", value);
        }

        public string? SubAutoExts
        {
            get => GetPropertyString("sub-auto-exts");
            set => SetProperty("sub-auto-exts", value);
        }

        public string? SubCodepage
        {
            get => GetPropertyString("sub-codepage");
            set => SetProperty("sub-codepage", value);
        }

        public bool SubStretchDurations
        {
            get => GetPropertyBoolean("sub-stretch-durations");
            set => SetProperty("sub-stretch-durations", value);
        }

        public bool SubFixTiming
        {
            get => GetPropertyBoolean("sub-fix-timing");
            set => SetProperty("sub-fix-timing", value);
        }

        public bool SubForcedEventsOnly
        {
            get => GetPropertyBoolean("sub-forced-events-only");
            set => SetProperty("sub-forced-events-only", value);
        }

        public double SubFps
        {
            get => GetPropertyDouble("sub-fps");
            set => SetProperty("sub-fps", value);
        }

        public double SubGauss
        {
            get => GetPropertyDouble("sub-gauss");
            set => SetProperty("sub-gauss", value);
        }

        public bool SubGray
        {
            get => GetPropertyBoolean("sub-gray");
            set => SetProperty("sub-gray", value);
        }

        public string? SubFilePaths
        {
            get => GetPropertyString("sub-file-paths");
            set => SetProperty("sub-file-paths", value);
        }

        public bool SubVisibility
        {
            get => GetPropertyBoolean("sub-visibility");
            set => SetProperty("sub-visibility", value);
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
