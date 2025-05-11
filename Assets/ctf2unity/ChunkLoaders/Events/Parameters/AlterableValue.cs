#if UNITY_EDITOR
class AlterableValue : Short
{

    public AlterableValue(ByteReader reader) : base(reader) { }
    public override void Read()
    {
        base.Read();

    }


}
#endif
