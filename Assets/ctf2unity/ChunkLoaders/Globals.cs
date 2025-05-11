#if UNITY_EDITOR
using System;
using System.Collections.Generic;

public class GlobalValues : ChunkLoader
{
    public List<float> Items = new List<float>();


    public override void Read()
    {
        var numberOfItems = Reader.ReadUInt16();
        var templist = new List<ByteReader>();
        for (int i = 0; i < numberOfItems; i++)
        {
            templist.Add(new ByteReader(Reader.ReadBytes(4)));
        }
        foreach (var item in templist)
        {
            var globalType = Reader.ReadSByte();
            float newGlobal = 0f;
            if (globalType == 2)
            {
                newGlobal = item.ReadSingle();
            }
            else if (globalType == 0)
            {
                newGlobal = item.ReadInt32();
            }
            else
            {
                throw new Exception("unknown global type");
            }
            Items.Add(newGlobal);
        }


    }

    public GlobalValues(ByteReader reader) : base(reader)
    {
    }
}
public class GlobalStrings : ChunkLoader
{
    public List<string> Items = new List<string>();


    public override void Read()
    {
        if (Reader.Tell() > Reader.Size() - 10)
        {
            Console.WriteLine("E80:  Ran out of bytes reading Globals (" + Reader.Tell() + "/" + Reader.Size() + ")");
            return; //really hacky shit, but it works
        }
        var count = Reader.ReadUInt32();
        for (int i = 0; i < count; i++)
        {
            Items.Add(Reader.ReadAscii());
        }

    }

    public GlobalStrings(ByteReader reader) : base(reader)
    {
    }
}
#endif