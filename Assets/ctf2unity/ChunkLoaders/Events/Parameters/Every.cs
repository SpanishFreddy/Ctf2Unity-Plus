#if UNITY_EDITOR
class Every : ParameterCommon
{
    public int Delay;
    public int Compteur;

    public Every(ByteReader reader) : base(reader) { }
    public override void Read()
    {
        Delay = Reader.ReadInt32();
        Compteur = Reader.ReadInt32();

    }


}
#endif
