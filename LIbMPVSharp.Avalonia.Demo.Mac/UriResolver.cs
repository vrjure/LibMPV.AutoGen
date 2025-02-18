using LibMPVSharp.Avalonia.Demo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMPVSharp.Avalonia.Demo.Mac
{
    internal class UriResolver : IUriResolver
    {
        public string? GetRealPath(Uri uri)
        {
            return uri.LocalPath;
        }
    }
}
