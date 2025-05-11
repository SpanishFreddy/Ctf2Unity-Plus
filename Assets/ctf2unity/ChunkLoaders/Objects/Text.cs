#if UNITY_EDITOR
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Text : ChunkLoader
{
    public int Width;
    public int Height;
    public List<Paragraph> Items = new List<Paragraph>();

    public Text(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {


        var currentPos = Reader.Tell();
        var size = Reader.ReadInt32();
        Width = Reader.ReadInt32();
        Height = Reader.ReadInt32();
        List<int> itemOffsets = new List<int>();
        var offCount = Reader.ReadInt32();
        for (int i = 0; i < offCount; i++)
        {
            itemOffsets.Add(Reader.ReadInt32());
        }
        foreach (int itemOffset in itemOffsets)
        {
            Reader.Seek(currentPos + itemOffset);
            var par = new Paragraph(Reader);
            par.Read();
            Items.Add(par);
        }



    }


}

public class Paragraph : ChunkLoader
{
    public ushort FontHandle;
    public BitDict Flags = new BitDict(new string[]{
            "HorizontalCenter",
            "RightAligned",
            "VerticalCenter",
            "BottomAligned",
            "None", "None", "None", "None",
            "Correct",
            "Relief"});
    public string Value;
    public Color32 Color;

    public Paragraph(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {


        FontHandle = Reader.ReadUInt16();
        Flags.flag = Reader.ReadUInt16();
        Color = Reader.ReadColor();
        Value = Reader.ReadWideString();


    }


}
#endif