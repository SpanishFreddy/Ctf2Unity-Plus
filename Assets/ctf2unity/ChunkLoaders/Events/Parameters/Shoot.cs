#if UNITY_EDITOR
public class Shoot : ParameterCommon
{
    public Position ShootPos;
    public ushort ObjectInstance;
    public ushort ObjectInfo;
    public short ShootSpeed;

    public Shoot(ByteReader reader) : base(reader)
    {
    }

    public override void Read()
    {
        ShootPos = new Position(Reader);
        ShootPos.Read();
        ObjectInstance = Reader.ReadUInt16();
        ObjectInfo = Reader.ReadUInt16();
        Reader.Skip(4);
        ShootSpeed = Reader.ReadInt16();
    }


}
#endif