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
        public static readonly double SpeedMin = 0.01d;
        public static readonly double SpeedMax = 100;

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
            get => GetPropertyString("seeking") == "yes";
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

        public double Speed
        {
            get => GetPropertyDouble("speed");
            set => SetProperty("speed", value);
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

        public long Volume
        {
            get => GetPropertyLong("volume");
            set => SetProperty("voume", value);
        }

        public long VolumeMax
        {
            get => GetPropertyLong("volume-max");
            set => SetProperty("volume-max", value);
        }

        public bool Mute
        {
            get => GetPropertyString("mute") == "yes";
            set => SetProperty("mute", value ? "yes" : "no");
        }

        public bool Pause
        {
            get => GetPropertyString("pause") == "yes";
            set => SetProperty("pause", value ? "yse" : "no");
        }

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
            get => GetPropertyString("screenshot-sw") == "yes";
            set => SetProperty("screenshot-sw", value ? "yes" : "no");
        }

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
            get => GetPropertyString("cache-on-disk") == "yes";
            set => SetProperty("cache-on-disk", value ? "yes" : "no");
        }

        public string DemuxerCacheDir
        {
            get => GetPropertyString("demuxer-cache-dir");
            set => SetProperty("demuxer-cache-dir", value);
        }

        public bool CachePause
        {
            get => GetPropertyString("cache-pause") == "yes";
            set => SetProperty("cache-pause", value ? "yes" : "no");
        }

        public long CachePauseWait
        {
            get => GetPropertyLong("cache-pause-wait");
            set => SetProperty("cache-pause-wait", value);
        }

        public bool CachePauseInitial
        {
            get => GetPropertyString("cache-pause-initial") == "yes";
            set => SetProperty("cache-pause-initial", value ? "yes" : "no");
        }

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
            get => GetPropertyString("cookies") == "yes";
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
    }
}
