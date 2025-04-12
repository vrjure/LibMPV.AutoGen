using System;
using System.Runtime.InteropServices.Marshalling;

namespace LibMPVSharp
{
    public unsafe static class MpvEventPropertyExtensions
    {
        public static string ReadStringValue(this ref MpvEventProperty property)
        {
            CheckValueFormat(property.name, property.format, MpvFormat.MPV_FORMAT_STRING);
            return Utf8StringMarshaller.ConvertToManaged((byte*)property.data);
        }

        public static long ReadLongValue(this ref MpvEventProperty property)
        {
            CheckValueFormat(property.name, property.format, MpvFormat.MPV_FORMAT_INT64);
            return *(long*)property.data;
        }

        public static bool ReadBoolValue(this ref MpvEventProperty property)
        {
            CheckValueFormat( property.name, property.format, MpvFormat.MPV_FORMAT_FLAG);
            return *(bool*)property.data;
        }

        public static double ReadDoubleValue(this ref MpvEventProperty property)
        {
            CheckValueFormat(property.name, property.format, MpvFormat.MPV_FORMAT_DOUBLE);
            return *(double*)property.data;
        }

        private static void CheckValueFormat(string name, MpvFormat format, MpvFormat required)
        {
            if (format != required) throw new FormatException($"The property '{name}' format is {format}, but require {required}");
        }
    }
}
