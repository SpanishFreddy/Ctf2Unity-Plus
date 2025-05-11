#if UNITY_EDITOR
class Short : ParameterCommon
{
    public short Value;

    public Short(ByteReader reader) : base(reader) { }
    public override void Read()
    {
        Value = Reader.ReadInt16();

    }


}
#endif
