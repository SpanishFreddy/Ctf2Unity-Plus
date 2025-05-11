#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static ChunkList;

class FrameName : StringChunk
{
    public FrameName(ByteReader reader) : base(reader)
    {
    }


}

class FramePassword : StringChunk
{
    public FramePassword(ByteReader reader) : base(reader)
    {
    }


}

public class Frame : ChunkLoader
{
    public BitDict Flags = new BitDict(new string[]
    {
           "XCoefficient",
           "YCoefficient",
           "DoNotSaveBackground",
           "Wrap",
           "Visible",
           "WrapHorizontally",
           "WrapVertically",
           "","","","","","","","","",
           "Redraw",
           "ToHide",
           "ToShow"
    });






    public override void Read()
    {
        //var frameReader = new ByteReader(Reader.ReadBytes());

        while (true)
        {
            var chunkId = Reader.ReadInt16();           
            var chunkFlag = (ChunkFlags)Reader.ReadInt16();
            var chunkSize = Reader.ReadInt32();
            if (chunkId == 32639) break;
            byte[] chunkData = Chunk.LoadChunkData(Reader.ReadBytes(chunkSize), chunkFlag, chunkId);
            if (chunkData == null) throw new InvalidDataException("Invalid ChunkData");
            var chunkReader = new ByteReader(chunkData);
            switch (chunkId)
            {
                case 13108:
                    var hdr = new FrameHeader(chunkReader);
                    hdr.Read();
                    Width = hdr.Width;
                    Height = hdr.Height;
                    Background = hdr.Background;
                    Flags = hdr.Flags;
                    break;
                case 13109:
                    var name = new FrameName(chunkReader);
                    name.Read();
                    Name = name.Value;
                    break;
                case 13111:
                    var palette = new FramePalette(chunkReader);
                    palette.Read();
                    Palette = palette.Items;
                    break;
                case 13112:
                    var objects = new ObjectInstances(chunkReader);
                    objects.Read();
                    Objects = objects.Items;
                    break;
                case 13127:
                    var tmr = new MovementTimerBase(chunkReader);
                    tmr.Read();
                    MovementTimer = tmr.Value;
                    break;
                case 13122:
                    var rect = new VirtualRect(chunkReader);
                    rect.Read();
                    VirtWidth = rect.Right;
                    VirtHeight = rect.Bottom;
                    break;
                case 13121:
                    var layers = new Layers(chunkReader);
                    layers.Read();
                    Layers = layers.Items;
                    break;
                case 13115:
                    var fIn = new Transition(chunkReader);
                    FadeIn = fIn;
                    break;
                case 13116:
                    var fOut = new Transition(chunkReader);
                    fOut.Read();
                    FadeOut = fOut;
                    break;
                case 13117:
                    var evts = new Events(chunkReader);
                    evts.Read();
                    Events = evts;
                    break;

                default:
                    break;
            }
            chunkReader.Close();

        }
        


    }

    public int Width { get; set; }
    public int Height { get; set; }
    public int VirtWidth { get; set; }
    public int VirtHeight { get; set; }
    public int MovementTimer { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public Color32 Background { get; set; }
    public List<ObjectInstanceReader> Objects { get; set; }
    public List<Color32> Palette { get; set; }
    public Events Events { get; set; }
    public Transition FadeIn { get; set; }
    public Transition FadeOut { get; set; }
    public List<Layer> Layers { get; set; }


    public Frame(ByteReader reader) : base(reader) { }


}

public class FrameHeader : ChunkLoader
{
    public int Width;
    public int Height;
    public BitDict Flags = new BitDict(new string[]
    {
            "XCoefficient",
            "YCoefficient",
            "DoNotSaveBackground",
            "Wrap",
            "Visible",
            "WrapHorizontally",
            "WrapVertically","","","","","","","","","",
            "Redraw",
            "ToHide",
            "ToShow"

    });
    public Color32 Background;
    public FrameHeader(ByteReader reader) : base(reader)
    {
    }




    public override void Read()
    {

        Width = Reader.ReadInt32();
        Height = Reader.ReadInt32();
        Background = Reader.ReadColor();
        Flags.flag = Reader.ReadUInt32();
    }
}
public class ObjectInstances : ChunkLoader
{
    public List<ObjectInstanceReader> Items = new List<ObjectInstanceReader>();

    public ObjectInstances(ByteReader reader) : base(reader)
    {
    }





    public override void Read()
    {

        var count = Reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            var item = new ObjectInstanceReader(Reader);
            item.Read();
            Items.Add(item);
        }
        Reader.Skip(4);
    }
}

public class ObjectInstanceReader : ChunkLoader
{
    public ushort handle;
    public ushort ObjectInfoHandle;
    public int X;
    public int Y;
    public short ParentType;
    public short Layer;
    public short ParentHandle;

    public ObjectInstanceReader(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {
        handle = (ushort)Reader.ReadInt16();
        ObjectInfoHandle = (ushort)Reader.ReadInt16();
        X = Reader.ReadInt32();
        Y = Reader.ReadInt32();
        ParentType = Reader.ReadInt16();
        ParentHandle = Reader.ReadInt16();
        Layer = Reader.ReadInt16();
        var res = Reader.ReadInt16();
    }




    public ObjectInfoReader FrameItem
    {
        get
        {
            return null;//Program.CleanData.Frameitems.FromHandle(ObjectInfo);
        }
    }

    public string Name => FrameItem.Name;



}

public class Layers : ChunkLoader
{
    public List<Layer> Items;

    public Layers(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {
        Items = new List<Layer>();

        var count = Reader.ReadUInt32();
        for (int i = 0; i < count; i++)
        {
            Layer item = new Layer(Reader);
            item.Read();
            Items.Add(item);
        }

    }


}

public class Layer : ChunkLoader
{
    public string Name;
    public BitDict Flags = new BitDict(new string[]
    {
            "XCoefficient",
            "YCoefficient",
            "DoNotSaveBackground",
            "",
            "Visible",
            "WrapHorizontally",
            "WrapVertically",
            "", "", "", "",
            "", "", "", "", "",
            "Redraw",
            "ToHide",
            "ToShow"
    }

    );
    public float XCoeff;
    public float YCoeff;
    public int NumberOfBackgrounds;
    public int BackgroudIndex;


    public Layer(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {
        Flags.flag = Reader.ReadUInt32();
        XCoeff = Reader.ReadSingle();
        YCoeff = Reader.ReadSingle();
        NumberOfBackgrounds = Reader.ReadInt32();
        BackgroudIndex = Reader.ReadInt32();
        Name = Reader.ReadWideString();
    }


}

public class FramePalette : ChunkLoader
{
    public List<Color32> Items;

    public FramePalette(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {
        if (Reader.Size() < 4)
        {
            Console.WriteLine("E445: Ran out of bytes reading Frame (" + Reader.Tell() + "/" + Reader.Size() + ")");
            return; //really hacky shit, but it works
        }
        Items = new List<Color32>();
        for (int i = 0; i < 257; i++)
        {
            Items.Add(Reader.ReadColor());
        }
    }


}
public class VirtualRect : Rect
{
    public VirtualRect(ByteReader reader) : base(reader) { }
}
public class MovementTimerBase : ChunkLoader
{
    public int Value;

    public MovementTimerBase(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {
        Value = Reader.ReadInt32();
    }



}
#endif