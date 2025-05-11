#if UNITY_EDITOR
public class Zone : ParameterCommon
{
    public short X1;
    public short Y1;
    public short X2;
    public short Y2;

    public Zone(ByteReader reader) : base(reader)
    {
    }

    public override void Read()
    {
        X1 = Reader.ReadInt16();
        Y1 = Reader.ReadInt16();
        X2 = Reader.ReadInt16();
        Y2 = Reader.ReadInt16();
    }



    public override string ToString()
    {
        return $"Zone ({X1}x{Y1})x({X2}x{Y2})";
    }
}
#endif