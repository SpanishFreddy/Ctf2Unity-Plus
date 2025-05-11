#if UNITY_EDITOR
public class TwoShorts : ParameterCommon
{
    public short Value1;
    public short Value2;

    public TwoShorts(ByteReader reader) : base(reader)
    {
    }

    public override void Read()
    {
        Value1 = Reader.ReadInt16();
        Value2 = Reader.ReadInt16();
    }


}
#endif