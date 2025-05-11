#if UNITY_EDITOR
class Float : ParameterCommon
{
    public float Value;

    public Float(ByteReader reader) : base(reader) { }
    public override void Read()
    {
        Value = Reader.ReadSingle();

    }

}
#endif