#if UNITY_EDITOR
class Int : Short
{
    public new int Value;

    public Int(ByteReader reader) : base(reader) { }
    public override void Read()
    {
        Value = Reader.ReadInt32();
    }


}
#endif
