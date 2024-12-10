using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMPV.AutoGen.Generation
{
    internal class GeneratorOption
    {
        public Dictionary<string, string> NameConverters { get; } = new Dictionary<string, string>()
        {
            { "event", "@event" },
            { "string", "@string" },
            { "params", "@params" }
        };

        public Dictionary<string, string> TypeConverters { get; } = new Dictionary<string, string>()
        {
            {"__IntPtr", "IntPtr" },
            {"char*", "string" },
            {"global::LibMPVSharp.Delegates.Action___IntPtr", "MpvSetWakeupCallback_cbCallback" },
            {"global::LibMPVSharp.Delegates.Func___IntPtr___IntPtr_string8", "MpvOpenglInitParams_get_proc_addressCallback" }
        };

        public Dictionary<string, IEnumerable<string>> AddOnDelegates { get; } = new Dictionary<string, IEnumerable<string>>()
        {
            {"MpvSetWakeupCallback_cbCallback", new[]{"[UnmanagedFunctionPointer(CallingConvention.Cdecl)]", "public unsafe delegate void MpvSetWakeupCallback_cbCallback(IntPtr arg);" } },
            {"MpvOpenglInitParams_get_proc_addressCallback", new[]{"[UnmanagedFunctionPointer(CallingConvention.Cdecl)]", "public unsafe delegate IntPtr MpvOpenglInitParams_get_proc_addressCallback(IntPtr ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string name);" } }
        };

    }
}
