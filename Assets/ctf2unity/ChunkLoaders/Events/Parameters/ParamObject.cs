#if UNITY_EDITOR
class ParamObject : ParameterCommon
{
    public int ObjectInfoList;
    public int ObjectInfo;
    public int ObjectType;
    public ParamObject(ByteReader reader) : base(reader) { }
    public override void Read()
    {

        ObjectInfoList = Reader.ReadInt16();
        ObjectInfo = Reader.ReadUInt16();
        ObjectType = Reader.ReadInt16();
    }


}
#endif
