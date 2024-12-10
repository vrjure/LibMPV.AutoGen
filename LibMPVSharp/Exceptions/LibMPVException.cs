using System;
using System.Collections.Generic;
using System.Text;

namespace LibMPVSharp
{
    public class LibMPVException : Exception
    {
        public MpvError Error { get; }

        public LibMPVException(MpvError error, string message = "") :base(message)
        {
            Error = error;
        }
    }
}
 