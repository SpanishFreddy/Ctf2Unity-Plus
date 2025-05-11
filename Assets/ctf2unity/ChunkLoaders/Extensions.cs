#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


public class Extensions : ChunkLoader
{
    internal ushort PreloadExtensions;
    public List<Extension> Items;

    public Extensions(ByteReader reader) : base(reader)
    {
    }


    public override void Read()
    {

        var count = Reader.ReadUInt16();
        PreloadExtensions = Reader.ReadUInt16();
        Items = new List<Extension>();
        for (int i = 0; i < count; i++)
        {
            var ext = new Extension(Reader);
            ext.Read();
            Items.Add(ext);
        }
    }


}
public class Extension : ChunkLoader
{
    public short Handle;
    public int MagicNumber;
    public int VersionLs;
    public int VersionMs;
    public string Name;
    public string Ext;
    public string SubType;

    public Extension(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {

    }


}
#endif