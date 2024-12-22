using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace LibMPVSharp
{
    public unsafe partial class MPVMediaPlayer
    {
        public delegate void PropertyChanged(ref MpvEventProperty property);
        public event PropertyChanged? MPVPropertyChanged;
    }
}
