using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LibMPVSharp;

public static class MPVNodeExtensions
{
    public unsafe static Dictionary<string, MpvNode> ReadNodeMap(this MpvNode node)
    {
        if (node.format != MpvFormat.MPV_FORMAT_NODE_MAP) throw new LibMPVException(MpvError.MPV_ERROR_PROPERTY_FORMAT ,"MPV format must be MPV_FORMAT_NODE_MAP"); 
        
        Dictionary<string, MpvNode> map = new();
        
        var size = Marshal.SizeOf<MpvNodeList>();
        var nodeList = Marshal.PtrToStructure<MpvNodeList>((IntPtr)node.u);
        
        var num = nodeList.num;
        var keys = nodeList.keys;
        var values = nodeList.values;
        for (int i = 0; i < num; i++)
        {
            var key = *(keys + i);
            var item = *(values + i);
            map.Add(Marshal.PtrToStringUTF8((IntPtr)key)!, item);
        }

        return map;
    }

    public unsafe static MpvNode[] ReadNodeArray(this MpvNode node)
    {
        if (node.format != MpvFormat.MPV_FORMAT_NODE_ARRAY) throw new LibMPVException(MpvError.MPV_ERROR_PROPERTY_FORMAT, "MPV format must be MPV_FORMAT_NODE_ARRAY");

        var size = Marshal.SizeOf<MpvNodeList>();
        var nodelist = Marshal.PtrToStructure<MpvNodeList>((IntPtr)node.u);
        
        var num = nodelist.num;
        var values = nodelist.values;
        
        var result = new MpvNode[num];
        for (int i = 0; i < num; i++)
        {
            result[i] = *(values + i);
        }
        
        return result;
    }

    public unsafe static string? ReadString(this MpvNode node)
    {
        if (node.format != MpvFormat.MPV_FORMAT_STRING) throw new LibMPVException(MpvError.MPV_ERROR_PROPERTY_FORMAT, "MPV format must be MPV_FORMAT_STRING");
        return Marshal.PtrToStringUTF8((IntPtr)node.u);
    }

    public unsafe static bool ReadBoolean(this MpvNode node)
    {
        if (node.format != MpvFormat.MPV_FORMAT_FLAG) throw new LibMPVException(MpvError.MPV_ERROR_PROPERTY_FORMAT, "MPV format must be MPV_FORMAT_FLAG");
        return (long)node.u == 1;
    }

    public unsafe static double ReadDouble(this MpvNode node)
    {
        if (node.format != MpvFormat.MPV_FORMAT_DOUBLE) throw new LibMPVException(MpvError.MPV_ERROR_PROPERTY_FORMAT, "MPV format must be MPV_FORMAT_DOUBLE");
        return (double)(IntPtr)node.u;
    }

    public unsafe static long ReadInt64(this MpvNode node)
    {
        if (node.format != MpvFormat.MPV_FORMAT_INT64) throw new LibMPVException(MpvError.MPV_ERROR_PROPERTY_FORMAT, "MPV format must be MPV_FORMAT_INT64");
        return (long)node.u;
    }

    public static void ReadToWriter(this MpvNode node, IndentedTextWriter writer)
    {
        switch (node.format)
        {
            case MpvFormat.MPV_FORMAT_STRING:
                writer.WriteLine(ReadString(node));
                break;
            case MpvFormat.MPV_FORMAT_INT64:
                writer.WriteLine(ReadInt64(node));
                break;
            case MpvFormat.MPV_FORMAT_DOUBLE:
                writer.WriteLine(ReadDouble(node));
                break;
            case MpvFormat.MPV_FORMAT_FLAG:
                writer.WriteLine(ReadBoolean(node));
                break;
            case MpvFormat.MPV_FORMAT_NODE_MAP:
                var map = ReadNodeMap(node);
                foreach (var item in map)
                {
                    var value = item.Value;
                    writer.Write("{0}:", item.Key);
                    if (item.Value.format == MpvFormat.MPV_FORMAT_NODE_MAP
                        || item.Value.format == MpvFormat.MPV_FORMAT_NODE_ARRAY)
                    {
                        writer.WriteLine();
                        writer.Indent++;
                        ReadToWriter(value, writer);
                        writer.Indent--;
                    }
                    else
                    {
                        ReadToWriter(value, writer);
                    }
                }
                break;
            case MpvFormat.MPV_FORMAT_NODE_ARRAY:
                var array = ReadNodeArray(node);
                foreach (var item in array)
                {
                    var value = item;
                    ReadToWriter(value, writer);
                }
                break;
        }
    }
}