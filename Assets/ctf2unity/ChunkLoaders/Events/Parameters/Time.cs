#if UNITY_EDITOR
class Time : ParameterCommon
{
    public int Timer;
    public int Loops;

    public Time(ByteReader reader) : base(reader) { }
    public override void Read()
    {
        Timer = Reader.ReadInt32();
        Loops = Reader.ReadInt32();

    }


}
#endif
