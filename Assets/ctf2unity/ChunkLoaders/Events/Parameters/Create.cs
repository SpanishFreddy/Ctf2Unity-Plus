#if UNITY_EDITOR
class Create : ParameterCommon
{
    public int ObjectInstances;
    public int ObjectInfo;
    public Position Position;

    public Create(ByteReader reader) : base(reader) { }
    public override void Read()
    {
        Position = new Position(Reader);
        Position.Read();
        ObjectInstances = Reader.ReadUInt16();
        ObjectInfo = Reader.ReadUInt16();
        // Reader.Skip(4);
    }


}
#endif
