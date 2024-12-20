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
        public string FileName
        {
            get => GetPropertyString("filename");
        }

        public string Path
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

        public string HwDec
        {
            get => GetPropertyString("hwdec");
            set => SetProperty("hwdec", value);
        }

        public string MPVVersion
        {
            get => GetPropertyString("mpv-version");
        }

        public string FFMpegVersion
        {
            get => GetPropertyString("ffmpeg-version");
        }

        public long LibASSVersion
        {
            get => GetPropertyLong("libass-version");
        }

        public string Platform
        {
            get => GetPropertyString("platform");
        }
        #endregion

        #region playback control
        public double Speed
        {
            get => GetPropertyDouble("speed");
            set => SetProperty("speed", value);
        }

        public bool Pause
        {
            get => GetPropertyBoolean("pause");
            set => SetProperty("pause", value);
        }
        #endregion


        #region Video
        public string VideoAspectOverride
        {
            get => GetPropertyString("video-aspect-override");
            set => SetProperty("video-aspect-override", value);
        }

        public string VideoUnscaled
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
        public string ScreenshotFormat
        {
            get => GetPropertyString("screenshot-format");
            set => SetProperty("screenshot-format", value);
        }

        public string ScreenshotTemplate
        {
            get => GetPropertyString("screenshot-template");
            set => SetProperty("screenshot-template", value);
        }

        public string ScreenshotDir
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
        public string Cache
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

        public string DemuxerCacheDir
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

        public string UserAgent
        {
            get => GetPropertyString("user-agent");
            set => SetProperty("user-agent", value);
        }

        public bool Cookies
        {
            get => GetPropertyBoolean("cookies");
            set => SetProperty("cookies", value);
        }

        public string CookiesFile
        {
            get => GetPropertyString("cookies-file");
            set => SetProperty("cookies-file", value);
        }

        public string HttpHeaderFields
        {
            get => GetPropertyString("http-header-fields");
            set => SetProperty("http-header-fields", value);
        }

        public string HttpProxy
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
