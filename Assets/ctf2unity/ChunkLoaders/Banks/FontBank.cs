#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;

public class FontBank : ChunkLoader
{
    public bool Compressed;
    public bool Debug;
    public List<FontItem> Items;



    public override void Read()
    {
        if (Settings.GameType == GameType.OnePointFive && !Settings.DoMFA) return;
        if (Settings.GameType == GameType.Android && !Settings.DoMFA) return;
        if (Debug)
        {
            //TODO
        }
        var count = Reader.ReadInt32();
        int offset = 0;
        if (Settings.Build > 284 && !Debug) offset = -1;

        Items = new List<FontItem>();
        for (int i = 0; i < count; i++)
        {
            var item = new FontItem(Reader);
            item.Read();
            item.Handle += (uint)offset;
            Items.Add(item);
        }


    }

    public FontBank(ByteReader reader) : base(reader)
    {
    }


}
public class FontItem : ChunkLoader
{
    public bool Compressed;
    public uint Handle;
    public int Checksum;
    public int References;
    public LogFont Value;

    public FontItem(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {
        Handle = Reader.ReadUInt32();
        var dataReader = Decompressor.DecompressAsReader(Reader, out var decompSize);
        var currentPos = dataReader.Tell();
        Checksum = dataReader.ReadInt32();
        References = dataReader.ReadInt32();
        var size = dataReader.ReadInt32();
        Value = new LogFont(dataReader);
        Value.Read();


    }


}

public class LogFont : ChunkLoader
{
    private int _height;
    private int _width;
    private int _escapement;
    private int _orientation;
    private int _weight;
    private byte _italic;
    private byte _underline;
    private byte _strikeOut;
    private byte _charSet;
    private byte _outPrecision;
    private byte _clipPrecision;
    private byte _quality;
    private byte _pitchAndFamily;
    private string _faceName;

    public LogFont(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {
        _height = Reader.ReadInt32();
        _width = Reader.ReadInt32();
        _escapement = Reader.ReadInt32();
        _orientation = Reader.ReadInt32();
        _weight = Reader.ReadInt32();
        _italic = Reader.ReadByte();
        _underline = Reader.ReadByte();
        _strikeOut = Reader.ReadByte();
        _charSet = Reader.ReadByte();
        _outPrecision = Reader.ReadByte();
        _clipPrecision = Reader.ReadByte();
        _quality = Reader.ReadByte();
        _pitchAndFamily = Reader.ReadByte();
        _faceName = Reader.ReadWideString(32);
    }




}
#endif