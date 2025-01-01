using Android.Content;
using Android.Database;
using Android.Net;
using Avalonia.Android;
using LibMPVSharp.Avalonia.Demo.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndroidUri = Android.Net.Uri;
using AndroidProvider = Android.Provider;

namespace LibMPVSharp.Avalonia.Demo.Android
{
    internal class UriResolver : IUriResolver
    {
        private readonly AvaloniaActivity _activity;
        public UriResolver(AvaloniaActivity activity)
        {
            _activity = activity;
            
        }
        public string? GetRealPath(System.Uri uri)
        {
            var contentResolver = _activity.ContentResolver;
            var androidUri = AndroidUri.Parse(uri.ToString());
            if (androidUri == null) return uri.ToString();

            var mediauri = AndroidProvider.MediaStore.GetMediaUri(_activity, androidUri);
            using var cursor = contentResolver!.Query(mediauri, [AndroidProvider.MediaStore.Video.Media.InterfaceConsts.Data], null, null, null);
            if (cursor!.MoveToFirst())
            {
                var res = cursor.GetString(0);
                return res;
            }
            return uri.ToString();
        }
    }
}
