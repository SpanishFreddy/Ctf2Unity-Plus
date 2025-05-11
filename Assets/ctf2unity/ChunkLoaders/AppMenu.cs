#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;

public class AppMenu : ChunkLoader
{
    public List<AppMenuItem> Items = new List<AppMenuItem>();
    public List<byte> AccelShift;
    public List<short> AccelKey;
    public List<short> AccelId;

    public AppMenu(ByteReader reader) : base(reader)
    {
    }


    public override void Read()
    {
        long currentPosition = Reader.Tell();
        uint headerSize = Reader.ReadUInt32();
        int menuOffset = Reader.ReadInt32();
        int menuSize = Reader.ReadInt32();
        if (menuSize == 0) return;
        int accelOffset = Reader.ReadInt32();
        int accelSize = Reader.ReadInt32();
        Reader.Seek(currentPosition + menuOffset);
        Reader.Skip(4);

        Load(Reader);

        Reader.Seek(currentPosition + accelOffset);
        AccelShift = new List<byte>();
        AccelKey = new List<short>();
        AccelId = new List<short>();
        for (int i = 0; i < accelSize / 8; i++)
        {
            AccelShift.Add(Reader.ReadByte());
            Reader.Skip(1);
            AccelKey.Add(Reader.ReadInt16());
            AccelId.Add(Reader.ReadInt16());
            Reader.Skip(2);
        }
    }


    public void Load(ByteReader reader)
    {
        while (true)
        {
            AppMenuItem newItem = new AppMenuItem(reader);
            newItem.Read();
            Items.Add(newItem);

            // if (newItem.Name.Contains("About")) break;
            if (ByteFlag.GetFlag((uint)newItem.Flags, 4))
            {
                Load(reader);
            }

            if (ByteFlag.GetFlag((uint)newItem.Flags, 7))
            {
                break;
            }
        }
    }
}

public class AppMenuItem : ChunkLoader
{
    public string Name = "";
    public Int16 Flags = 0;
    public Int16 Id = 0;
    public string Mnemonic = null;

    public AppMenuItem(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {
        Flags = Reader.ReadInt16();
        if (!ByteFlag.GetFlag((uint)Flags, 4))
        {
            Id = Reader.ReadInt16();
        }

        Name = Reader.ReadWideString();

        for (int i = 0; i < Name.Length; i++)
        {
            if (Name[i] == '&')
            {
                Mnemonic = Name[i + 1].ToString();
                Name = Name.Replace("&", "");
                break;
            }
        }


    }


}
#endif