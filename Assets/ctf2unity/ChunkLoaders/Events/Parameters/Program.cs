#if UNITY_EDITOR
public class Program : ParameterCommon
{
    public short Flags;
    public string Filename;
    public string Command;

    public Program(ByteReader reader) : base(reader)
    {
    }

    public override void Read()
    {

        Flags = Reader.ReadInt16();
        Filename = Reader.ReadAscii(260);
        Command = Reader.ReadAscii();
    }


}
#endif