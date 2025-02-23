using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMPVSharp
{
    public static class LibraryName
    {
#if ANDROID
        public const string Name = "libmpv.so";
#elif MACOS
        public const string Name = "libmpv.2.dylib";
#else
        public const string Name = "libmpv-2";
#endif
    }
}
