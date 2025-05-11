#if UNITY_EDITOR
using System;

public class Rect : ChunkLoader
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;
    public Rect(ByteReader reader) : base(reader) { }

    public override void Read()
    {
        //if (Settings.GameType == GameType.TwoFivePlus) Console.WriteLine("position: " + Reader.Tell() + " size: " + Reader.Size());

        Left = Reader.ReadInt32();
        Top = Reader.ReadInt32();
        Right = Reader.ReadInt32();
        Bottom = Reader.ReadInt32();
    }


}
#endif