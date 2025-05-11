#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;


public class FrameHandles : ChunkLoader
{
    public Dictionary<int, int> Items;

    public FrameHandles(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {

        var len = Reader.Size() / 2;
        Items = new Dictionary<int, int>();
        for (int i = 0; i < len; i++)
        {
            var handle = Reader.ReadInt16();
            Items.Add(i, handle);
        }

    }



}
#endif