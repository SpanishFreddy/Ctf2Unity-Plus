#if UNITY_EDITOR
public class Group : ParameterCommon
{
    public long Offset;
    public ushort Flags;
    public ushort Id;
    public string Name;
    public int Password;
    public byte[] Unk1;
    public byte[] Unk2;

    public Group(ByteReader reader) : base(reader)
    {
    }

    public override void Read()
    {
        Offset = Reader.Tell() - 24;
        Flags = Reader.ReadUInt16();
        Id = Reader.ReadUInt16();
        Name = Reader.ReadWideString();
        Unk1 = Reader.ReadBytes(190 - Name.Length * 2);
        Password = Reader.ReadInt32();
        Reader.ReadInt16();

    }


}
#endif