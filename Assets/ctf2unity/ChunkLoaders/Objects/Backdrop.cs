using System;
using System.Drawing;
using UnityEngine;

public enum Obstacle
{
    None = 0,
    Solid = 1,
    Platform = 2,
    Ladder = 3,
    Transparent = 4
}

public enum Collision
{
    Fine = 0,
    Box = 1
}

#if UNITY_EDITOR
public class Backdrop : ChunkLoader
{

    public int Size;
    public Obstacle ObstacleType;
    public Collision CollisionType;
    public int Width;
    public int Height;
    public int Image;

    public Backdrop(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {


        if (Reader.Tell() > Reader.Size() - 18)
        {
            Console.WriteLine("E51:  Ran out of bytes reading Backdrop Object (" + Reader.Tell() + "/" + Reader.Size() + ")");
            return; //really hacky shit, but it works
        }
        Size = Reader.ReadInt32();
        ObstacleType = (Obstacle)Reader.ReadInt16();
        CollisionType = (Collision)Reader.ReadInt16();
        Width = Reader.ReadInt32();
        Height = Reader.ReadInt32();
        Image = Reader.ReadInt16();

    }




}
public class Quickbackdrop : ChunkLoader
{
    public int Size;
    public Obstacle ObstacleType;
    public Collision CollisionType;
    public int Width;
    public int Height;
    public int Image;
    public Shape Shape;

    public Quickbackdrop(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {


        if (Reader.Tell() > Reader.Size() - 20)
        {
            Console.WriteLine("E108: Ran out of bytes reading Backdrop Object (" + Reader.Tell() + "/" + Reader.Size() + ")");
            return; //really hacky shit, but it works
        }
        Size = Reader.ReadInt32();
        ObstacleType = (Obstacle)Reader.ReadInt16();
        CollisionType = (Collision)Reader.ReadInt16();
        Width = Reader.ReadInt32();
        Height = Reader.ReadInt32();
        Shape = new Shape(Reader);
        Shape.Read();


    }


}
public class Shape : ChunkLoader
{
    public short BorderSize;
    public Color32 BorderColor;
    public short ShapeType;
    public short FillType;
    public short LineFlags;
    public Color32 Color1;
    public Color32 Color2;
    public short GradFlags;
    public short Image = 15;

    public Shape(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {

        if (Reader.Tell() > Reader.Size() - 10)
        {
            Console.WriteLine("E155: Ran out of bytes reading Backdrop Object (" + Reader.Tell() + "/" + Reader.Size() + ")");
            return; //really hacky shit, but it works
        }
        BorderSize = Reader.ReadInt16();
        BorderColor = Reader.ReadColor();
        ShapeType = Reader.ReadInt16();
        FillType = Reader.ReadInt16();
        if (ShapeType == 1)
        {
            LineFlags = Reader.ReadInt16();
        }
        else if (FillType == 1)
        {
            Color1 = Reader.ReadColor();
        }
        else if (FillType == 2)
        {
            Color1 = Reader.ReadColor();
            Color2 = Reader.ReadColor();
            GradFlags = Reader.ReadInt16();
        }
        // else if(FillType==3)
        // {
        Image = Reader.ReadInt16();
        // }
    }


}
#endif