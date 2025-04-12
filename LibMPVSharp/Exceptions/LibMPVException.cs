using System;

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
 