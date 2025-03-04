using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibMPVSharp.Extensions
{
    public unsafe static class MpvEventExtensions
    {
        public static T ReadData<T>(this MpvEvent mpvEvent) where T: struct
        {
            return Marshal.PtrToStructure<T>((IntPtr)mpvEvent.data);
        }
    }
}
