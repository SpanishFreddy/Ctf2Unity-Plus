#if UNITY_EDITOR    
public class Position : ParameterCommon
{
    public int ObjectInfoParent;
    public short Flags;
    public int X;
    public int Y;
    public int Slope;
    public int Angle;
    public int Direction;
    public int TypeParent;
    public int ObjectInfoList;
    public int Layer;

    public Position(ByteReader reader) : base(reader) { }
    public override void Read()
    {
        ObjectInfoParent = Reader.ReadInt16();
        Flags = Reader.ReadInt16();
        X = Reader.ReadInt16();
        Y = Reader.ReadInt16();
        Slope = Reader.ReadInt16();
        Angle = Reader.ReadInt16();
        Direction = Reader.ReadInt32();
        TypeParent = Reader.ReadInt16();
        ObjectInfoList = Reader.ReadInt16();
        Layer = Reader.ReadInt16();
    }


}
#endif
