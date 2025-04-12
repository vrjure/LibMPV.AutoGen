using System;
using System.Runtime.InteropServices;

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
