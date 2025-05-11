#if UNITY_EDITOR
public class Expression : ChunkLoader
{
    public int ObjectType;
    public int Num;
    public int ObjectInfo;
    public int ObjectInfoList;
    public object value;
    public object floatValue;
    public ChunkLoader Loader;
    public int Unk1;
    public ushort Unk2;
    private int _unk;
    public Expression(ByteReader reader) : base(reader) { }

    public override void Read()
    {

        var currentPosition = Reader.Tell();
        var old = false;
        ObjectType = (old ? Reader.ReadSByte() : Reader.ReadInt16());
        Num = old ? Reader.ReadSByte() : Reader.ReadInt16();

        if (ObjectType == 0 && Num == 0) return;

        var size = Reader.ReadInt16();
        if (ObjectType == (int)Constants.ObjectType.System)
        {
            if (Num == 0) Loader = new LongExp(Reader);
            else if (Num == 3) Loader = new StringExp(Reader);
            else if (Num == 23) Loader = new DoubleExp(Reader);
            else if (Num == 24) Loader = new GlobalCommon(Reader);
            else if (Num == 50) Loader = new GlobalCommon(Reader);
            else if ((int)ObjectType >= 2 || (int)ObjectType == -7)
            {
                ObjectInfo = Reader.ReadUInt16();
                ObjectInfoList = Reader.ReadInt16();
                if (Num == 16 || Num == 19)
                {
                    Loader = new ExtensionExp(Reader);
                }
                else
                {
                    _unk = Reader.ReadInt32();
                }
            }
        }
        else if ((int)ObjectType >= 2 || (int)ObjectType == -7)
        {
            ObjectInfo = Reader.ReadUInt16();
            ObjectInfoList = Reader.ReadInt16();
            if (Num == 16 || Num == 19)
            {
                Loader = new ExtensionExp(Reader);
            }
        }
        Loader?.Read();
        // Unk1 = Reader.ReadInt32();
        // Unk2 = Reader.ReadUInt16();
        //if (Settings.GameType == GameType.TwoFivePlus) Console.WriteLine("EVENT current position: " + currentPosition + " size: " + size);
        if (currentPosition + size < 0) return;
        if (size <= 0) return;
        Reader.Seek(currentPosition + size);


    }

    public override string ToString()
    {
        return $"Expression {ObjectType}=={Num}: {((ExpressionLoader)Loader)?.Value}";
    }
}
public class ExpressionLoader : ChunkLoader
{
    public object Value;
    public ExpressionLoader(ByteReader reader) : base(reader)
    {
    }


    public override void Read()
    {
    }


}

public class StringExp : ExpressionLoader
{


    public StringExp(ByteReader reader) : base(reader)
    {
    }


    public override void Read()
    {
        Value = Reader.ReadWideString();
    }


}
public class LongExp : ExpressionLoader
{
    public int Val1;

    public LongExp(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {
        Value = Reader.ReadInt32();
    }

}
public class ExtensionExp : ExpressionLoader
{
    public ExtensionExp(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {
        Value = Reader.ReadInt16();
    }


}
public class DoubleExp : ExpressionLoader
{
    public float FloatValue;

    public DoubleExp(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {
        Value = Reader.ReadDouble();
        FloatValue = Reader.ReadSingle();
    }


}
public class GlobalCommon : ExpressionLoader
{
    public GlobalCommon(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {
        Reader.ReadInt32();
        Value = Reader.ReadInt32();
    }


}
#endif
