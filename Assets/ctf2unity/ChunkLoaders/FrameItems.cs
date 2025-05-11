#if UNITY_EDITOR
using System;
using System.Collections.Generic;

public class FrameItems : ChunkLoader
{
    public Dictionary<int, ObjectInfoReader> ItemDict = new Dictionary<int, ObjectInfoReader>();
    public FrameItems(ByteReader reader) : base(reader) { }


    public override void Read()
    {
        var count = Reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            var item = new ObjectInfoReader(Reader);
            item.Read();
            ItemDict.Add(item.Handle, item);
        }

    }

    public ObjectInfoReader FromHandle(int handle)
    {
        ItemDict.TryGetValue(handle, out var ret);
        return ret;
    }

    public List<ObjectInfoReader> FromName(string name)
    {
        var tempList = new List<ObjectInfoReader>();
        foreach (var key in ItemDict.Keys)
        {
            var item = ItemDict[key];
            if (item.Name == name) tempList.Add(item);
        }

        return tempList;
    }
}
#endif