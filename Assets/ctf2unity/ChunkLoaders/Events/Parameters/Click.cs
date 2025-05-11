#if UNITY_EDITOR
public class Click : ParameterCommon
{
    public byte IsDouble;
    public byte Button;

    public Click(ByteReader reader) : base(reader)
    {
    }

    public override void Read()
    {
        Button = Reader.ReadByte();
        IsDouble = Reader.ReadByte();
    }


}
#endif