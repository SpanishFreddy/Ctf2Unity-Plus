#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
public class SoundBank : ChunkLoader
{
    public int NumOfItems = 0;
    public int References = 0;
    public List<SoundItem> Items;
    public bool IsCompressed = true;


    public void Read(bool dump)
    {
        var cache = Settings.DumpSounds;
        Settings.DumpSounds = dump;
        Read();
        Settings.DumpSounds = cache;

    }
    public override void Read()
    {
        if (!Settings.DoMFA) Reader.Seek(0);//Reset the reader to avoid bugs when dumping more than once
        Items = new List<SoundItem>();
        NumOfItems = Reader.ReadInt32();
        //if (!Settings.DumpSounds) return;

        for (int i = 0; i < NumOfItems; i++)
        {

            var item = new SoundItem(Reader);

            item.IsCompressed = IsCompressed;
            item.Read();



            Items.Add(item);


        }

    }


    public SoundBank(ByteReader reader) : base(reader)
    {
    }


}

public class SoundBase : ChunkLoader
{



    public override void Read()
    {

    }

    public SoundBase(ByteReader reader) : base(reader)
    {
    }


}

public class SoundItem : SoundBase
{
    public int Checksum;
    public uint References;
    public uint Flags;
    public bool IsCompressed = false;
    public uint Handle;
    public string Name;
    public byte[] Data;


    public override void Read()
    {
        base.Read();

        var start = Reader.Tell();

        Handle = Reader.ReadUInt32();
        Checksum = Reader.ReadInt32();

        References = Reader.ReadUInt32();
        var decompressedSize = Reader.ReadInt32();
        Flags = Reader.ReadUInt32();
        var reserved = Reader.ReadInt32();
        var nameLenght = Reader.ReadInt32();
        ByteReader soundData;
        if (IsCompressed)
        {
            var size = Reader.ReadInt32();
            soundData = new ByteReader(Decompressor.DecompressBlock(Reader, size, decompressedSize));
        }
        else
        {
            soundData = new ByteReader(Reader.ReadBytes(decompressedSize));
        }
        Name = Settings.GameType == GameType.NSwitch ? soundData.ReadAscii(nameLenght) : soundData.ReadWideString(nameLenght);
        Name = Name.Replace(" ", "");
        Data = soundData.ReadBytes((int)soundData.Size());

    }



    public SoundItem(ByteReader reader) : base(reader)
    {
    }
}
#endif

