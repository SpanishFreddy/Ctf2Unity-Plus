#if UNITY_EDITOR
public class KeyParameter : ParameterCommon
{
    public short Key;

    public KeyParameter(ByteReader reader) : base(reader)
    {
    }

    public override void Read()
    {
        Key = Reader.ReadInt16();
    }



    public override string ToString()
    {
        return "Key-" + Key;
    }
}
#endif