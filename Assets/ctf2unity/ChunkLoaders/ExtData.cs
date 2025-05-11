#if UNITY_EDITOR
using System;
using System.IO;

class ExtData : ChunkLoader
{


    public override void Read()
    {
        var filename = Reader.ReadAscii();
        var data = Reader.ReadBytes();
        // File.WriteAllBytes($"{Settings.DumpPath}\\{filename}",data);
    }

    public ExtData(ByteReader reader) : base(reader)
    {
    }
}
#endif