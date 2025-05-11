#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
public class Movements : ChunkLoader
{
    public List<Movement> Items = new List<Movement>();

    public Movements(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {

        var rootPosition = Reader.Tell();
        var count = Reader.ReadUInt32();
        var currentPos = Reader.Tell();
        for (int i = 0; i < count; i++)
        {
            var mov = new Movement(Reader);
            mov.rootPos = (int)rootPosition;
            mov.Read();
            Items.Add(mov);
            Reader.Seek(currentPos + 16);
            currentPos = Reader.Tell();

        }

    }


}

public class Movement : ChunkLoader
{
    public static int DataSize;
    public int rootPos;
    public ushort Player;
    public ushort Type;
    public byte MovingAtStart;
    public int DirectionAtStart;
    public MovementLoader Loader;

    public Movement(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {


        var nameOffset = Reader.ReadInt32();
        var movementId = Reader.ReadInt32();
        var newOffset = Reader.ReadInt32();
        DataSize = Reader.ReadInt32();
        Reader.Seek(rootPos + newOffset);
        if (Reader.Tell() > Reader.Size() - 12)
        {
            Console.WriteLine("E122: Ran out of bytes reading Movement Object (" + Reader.Tell() + "/" + Reader.Size() + ")");
            return; //really hacky shit, but it works
        }
        Player = Reader.ReadUInt16();
        Type = Reader.ReadUInt16();
        MovingAtStart = Reader.ReadByte();
        Reader.Skip(3);
        DirectionAtStart = Reader.ReadInt32();
        switch (Type)
        {
            case 1:
                Loader = new Mouse(Reader);
                break;
            case 2:
                Loader = new RaceMovement(Reader);
                break;
            case 3:
                Loader = new EightDirections(Reader);
                break;
            case 4:
                Loader = new Ball(Reader);
                break;
            case 5:
                Loader = new MovementPath(Reader);
                break;
            case 9:
                Loader = new PlatformMovement(Reader);
                break;
            case 14:
                Loader = new ExtensionsMovement(Reader);
                break;


        }
        if (Loader == null && Type != 0) return; //throw new Exception("Unsupported movement: "+Type);
        Loader?.Read();


    }

}
public class MovementLoader : ChunkLoader
{
    public MovementLoader(ByteReader reader) : base(reader)
    {
    }

    public override void Read()
    {
        throw new System.NotImplementedException();
    }

}
public class Mouse : MovementLoader
{
    public short X1;
    public short X2;
    public short Y1;
    public short Y2;
    private short _unusedFlags;

    public Mouse(ByteReader reader) : base(reader)
    {
    }


    public override void Read()
    {

        X1 = Reader.ReadInt16();
        X2 = Reader.ReadInt16();
        Y1 = Reader.ReadInt16();
        Y2 = Reader.ReadInt16();
        _unusedFlags = Reader.ReadInt16();
    }


}
public class MovementPath : MovementLoader
{
    public short MinimumSpeed;
    public short MaximumSpeed;
    public byte Loop;
    public byte RepositionAtEnd;
    public byte ReverseAtEnd;
    public List<MovementStep> Steps;

    public MovementPath(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {

        var count = Reader.ReadInt16();
        MinimumSpeed = Reader.ReadInt16();
        MaximumSpeed = Reader.ReadInt16();
        Loop = Reader.ReadByte();
        RepositionAtEnd = Reader.ReadByte();
        ReverseAtEnd = Reader.ReadByte();
        Reader.Skip(1);
        Steps = new List<MovementStep>();
        for (int i = 0; i < count; i++)
        {
            var currentPosition = Reader.Tell();

            Reader.Skip(1);
            var size = Reader.ReadByte();
            var step = new MovementStep(Reader);
            step.Read();
            Steps.Add(step);
            Reader.Seek(currentPosition + size);
        }
    }


}
public class MovementStep : MovementLoader
{
    public byte Speed;
    public byte Direction;
    public short DestinationX;
    public short DestinationY;
    public short Cosinus;
    public short Sinus;
    public short Length;
    public short Pause;
    public string Name;

    public MovementStep(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {


        Speed = Reader.ReadByte();
        Direction = Reader.ReadByte();
        DestinationX = Reader.ReadInt16();
        DestinationY = Reader.ReadInt16();
        Cosinus = Reader.ReadInt16();
        Sinus = Reader.ReadInt16();
        Length = Reader.ReadInt16();
        Pause = Reader.ReadInt16();
        Name = Reader.ReadAscii();


    }


}
public class Ball : MovementLoader
{
    public short Speed;
    public short Randomizer;
    public short Angles;
    public short Security;
    public short Deceleration;

    public Ball(ByteReader reader) : base(reader)
    {
    }


    public override void Read()
    {
        Speed = Reader.ReadInt16();
        Randomizer = Reader.ReadInt16();
        Angles = Reader.ReadInt16();
        Security = Reader.ReadInt16();
        Deceleration = Reader.ReadInt16();


    }


}
public class EightDirections : MovementLoader
{
    public short Speed;
    public short Acceleration;
    public short Deceleration;
    public int Directions;
    public short BounceFactor;

    public EightDirections(ByteReader reader) : base(reader)
    {
    }


    public override void Read()
    {

        Speed = Reader.ReadInt16();
        Acceleration = Reader.ReadInt16();
        Deceleration = Reader.ReadInt16();
        BounceFactor = Reader.ReadInt16();
        Directions = Reader.ReadInt32();
    }


}
public class RaceMovement : MovementLoader
{
    public short Speed;
    public short Acceleration;
    public short Deceleration;
    public short RotationSpeed;
    public short BounceFactor;
    public short Angles;
    public short ReverseEnabled;

    public RaceMovement(ByteReader reader) : base(reader)
    {
    }


    public override void Read()
    {

        Speed = Reader.ReadInt16();
        Acceleration = Reader.ReadInt16();
        Deceleration = Reader.ReadInt16();
        RotationSpeed = Reader.ReadInt16();
        BounceFactor = Reader.ReadInt16();
        Angles = Reader.ReadInt16();
        ReverseEnabled = Reader.ReadInt16();
    }


}
public class PlatformMovement : MovementLoader
{
    public short Speed;
    public short Acceleration;
    public short Deceleration;
    public short Control;
    public short Gravity;
    public short JumpStrength;

    public PlatformMovement(ByteReader reader) : base(reader)
    {
    }


    public override void Read()
    {

        Speed = Reader.ReadInt16();
        Acceleration = Reader.ReadInt16();
        Deceleration = Reader.ReadInt16();
        Control = Reader.ReadInt16();
        Gravity = Reader.ReadInt16();
        JumpStrength = Reader.ReadInt16();
    }

}
public class ExtensionsMovement : MovementLoader
{
    public byte[] Data;

    public ExtensionsMovement(ByteReader reader) : base(reader)
    {
    }




}
#endif