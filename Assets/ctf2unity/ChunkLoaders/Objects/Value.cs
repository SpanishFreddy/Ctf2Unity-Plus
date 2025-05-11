#if UNITY_EDITOR
using System;
using System.Collections.Generic;

public class AlterableValues : ChunkLoader
{
    public List<int> Items;

    public AlterableValues(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {

        Items = new List<int>();
        Reader.ReadUInt16();
        var count = Reader.ReadUInt16();
        Console.WriteLine(count);
        for (int i = 0; i < count; i++)
        {

            var item = Reader.ReadInt32();

            Items.Add(item);

        }
    }

}

public class AlterableStrings : ChunkLoader
{
    public List<string> Items;

    public AlterableStrings(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {

        Items = new List<string>();

        var count = Reader.ReadUInt16();

        for (int i = 0; i < count; i++)
        {
            var item = Reader.ReadWideString();
            Items.Add(item);

        }
    }

}
#endif