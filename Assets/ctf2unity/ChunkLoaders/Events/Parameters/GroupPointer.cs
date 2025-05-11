#if UNITY_EDITOR
public class GroupPointer : ParameterCommon
{
    public int Pointer;
    public short Id;

    public GroupPointer(ByteReader reader) : base(reader)
    {
    }

    public override void Read()
    {
        Pointer = Reader.ReadInt32();
        Id = Reader.ReadInt16();

    }


}
#endif