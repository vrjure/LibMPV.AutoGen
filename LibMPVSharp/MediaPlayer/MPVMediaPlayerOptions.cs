using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMPVSharp
{
    public class MPVMediaPlayerOptions
    {
        public MpvOpenglInitParams_get_proc_addressCallback GetProcAddress { get; set; }
        public MpvRenderUpdateFn UpdateCallback { get; set; }
    }
}
