#if UNITY_EDITOR
class GlobalValue : Short
{


    public GlobalValue(ByteReader reader) : base(reader) { }
    public override void Read()
    {
        base.Read();
    }

}
#endif
