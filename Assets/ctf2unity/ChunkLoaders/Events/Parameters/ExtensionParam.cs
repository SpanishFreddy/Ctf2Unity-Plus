#if UNITY_EDITOR
public class ExtensionParam : ParameterCommon
{
    public short Size;
    public short Type;
    public short Code;
    public byte[] Data;

    public ExtensionParam(ByteReader reader) : base(reader)
    {
    }

    public override void Read()
    {
        Size = Reader.ReadInt16();
        Type = Reader.ReadInt16();
        Code = Reader.ReadInt16();
        Data = Reader.ReadBytes((Size - 20 > 0 ? Size - 20 : 0));

    }


}
#endif