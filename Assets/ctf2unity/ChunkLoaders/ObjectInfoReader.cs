#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static ChunkList;

public class ObjectInfoReader : ChunkLoader
{

    public List<Chunk> Chunks = new List<Chunk>();
    public int ShaderId;
    public int Items;
    private ObjectHeader _header = new ObjectHeader((ByteReader)null);
    private ObjectName _name = new ObjectName((ByteReader)null);
    private ObjectProperties _properties;
    public ObjectInfoReader(ByteReader reader) : base(reader) { }


    public override void Read()
    {
        var infoChunks = new ChunkList();
        infoChunks.Verbose = false;
        infoChunks.Read(Reader);
        _header = infoChunks.GetChunk<ObjectHeader>();
        _name = infoChunks.GetChunk<ObjectName>();
        _properties = infoChunks.GetChunk<ObjectProperties>();



        Handle = _header.Handle;
        ObjectType = (Constants.ObjectType)_header.ObjectType;
        Flags = (int)_header.Flags;
        InkEffect = _header.InkEffect & 0xFFFF;
        if (InkEffect >= 4096) InkEffect = InkEffect - 4096; //ctf2unity fix
        BlendCoeff = _header.InkEffectParameter;
        RGBCoeff = _header.RGBCoeff;
        Name = _name?.Value ?? $"{ObjectType}-{Handle}";
        Properties = _properties;
        Properties.ReadNew((int)ObjectType, this, true);




    }

    public int Handle { get; set; }

    public string Name { get; set; }

    public ObjectProperties Properties { get; set; }

    public Constants.ObjectType ObjectType { get; set; }

    public int Flags { get; set; }

    public int Reserved { get; set; }

    public int InkEffect { get; set; }

    public byte BlendCoeff { get; set; }
    public Color32 RGBCoeff { get; set; } 
    public bool Transparent => ByteFlag.GetFlag((uint)_header.InkEffect, 28);
    public bool Antialias => ByteFlag.GetFlag((uint)_header.InkEffect, 29);



    public List<ObjectInstanceReader> GetInstances()
    {
        var list = new List<ObjectInstanceReader>();
        /*var frames = Exe.Instance.GameData.frames;
        foreach (var frame in frames)
        {
            foreach (ObjectInstance instance in frame.Objects)
            {
                if(instance.ObjectInfo==this.Handle)list.Add(instance);
            }
        }*/

        return list;
    }
}

public class ObjectName : StringChunk
{
    public ObjectName(ByteReader reader) : base(reader) { }
}

public class ObjectProperties : ChunkLoader
{
    public bool IsCommon;
    public ChunkLoader Loader;

    public ObjectProperties(ByteReader reader) : base(reader) { }
    public void ReadNew(int ObjectType, ObjectInfoReader parent, bool isFirstRead)
    {
        if (ObjectType == 0) Loader = new Quickbackdrop(Reader);
        else if (ObjectType == 1) Loader = new Backdrop(Reader);
        else
        {
            IsCommon = true;

            Loader = new ObjectCommon(Reader, parent);
            ((ObjectCommon)Loader).isFirstRead = isFirstRead;
        }
        Loader?.Read();
    }
    public override void Read() { }


}

public class ObjectHeader : ChunkLoader
{
    public Int16 Handle;
    public Int16 ObjectType;
    public UInt32 Flags;
    public int InkEffect;
    public byte InkEffectParameter;
    public Color32 RGBCoeff;

    public ObjectHeader(ByteReader reader) : base(reader) { }


    public override void Read()
    {
        Handle = Reader.ReadInt16();
        ObjectType = Reader.ReadInt16();
        Flags = Reader.ReadUInt16();
        var Reserved = Reader.ReadInt16();
        InkEffect = (byte)Reader.ReadByte();
        RGBCoeff = new Color32(255, 255, 255, 0);
        if (InkEffect != 1)
        {
            var isBlended = Reader.ReadByte();
            Reader.Skip(2);
            
            var r = Reader.ReadByte();
            var g = Reader.ReadByte();
            var b = Reader.ReadByte();
            InkEffectParameter = (byte)(byte.MaxValue -Reader.ReadByte() );
        }
        else
        {
            var flag = Reader.ReadByte();
            Reader.Skip(2);
            InkEffectParameter = (byte)(Reader.ReadByte() * 2);
        }
    }
}
#endif