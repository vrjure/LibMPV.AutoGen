namespace LibMPVSharp;

public unsafe class MpvNodeWrap
{
    private MpvNodeWrap(){}

    internal MpvNodeWrap(MpvNode* native, MpvNode node)
    {
        Native = native;
        Node = node;
    }
    public MpvNode Node { get; }
    internal MpvNode* Native { get; }
}