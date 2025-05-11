#if UNITY_EDITOR
using System;
using System.Collections.Generic;

public class Animations : ChunkLoader
{
    public Dictionary<int, Animation> AnimationDict;

    public Animations(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {

        var currentPosition = Reader.Tell();
        var size = Reader.ReadInt16();
        var count = Reader.ReadInt16();
        //if (Settings.GameType == GameType.TwoFivePlus) Console.WriteLine("reading animation - size " + Reader.Size() + ", there are " + count + " imgs @ "+currentPosition);
        var offsets = new List<short>();
        for (int i = 0; i < count; i++)
        {
            offsets.Add(Reader.ReadInt16());
        }
        AnimationDict = new Dictionary<int, Animation>();
        for (int i = 0; i < offsets.Count; i++)
        {
            var offset = offsets[i];
            if (offset != 0)
            {
                Reader.Seek(currentPosition + offset);
                var anim = new Animation(Reader);

                anim.Read();
                AnimationDict.Add(i, anim);

            }
            else
            {
                AnimationDict.Add(i, new Animation((ByteReader)null));
            }

        }



    }


}

public class Animation : ChunkLoader
{
    public Dictionary<int, AnimationDirection> DirectionDict;

    public Animation(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {

        var currentPosition = Reader.Tell();
        var offsets = new List<int>();
        for (int i = 0; i < 32; i++)
        {
            offsets.Add(Reader.ReadInt16());
        }

        DirectionDict = new Dictionary<int, AnimationDirection>();
        for (int i = 0; i < offsets.Count; i++)
        {
            var offset = offsets[i];
            if (offset != 0)
            {
                Reader.Seek(currentPosition + offset);
                var dir = new AnimationDirection(Reader);
                dir.Read();
                DirectionDict.Add(i, dir);
            }
            else
            {
                DirectionDict.Add(i, new AnimationDirection((ByteReader)null));
            }

        }
    }


}

public class AnimationDirection : ChunkLoader
{
    public int MinSpeed;
    public int MaxSpeed;
    public bool HasSingle;
    public int Repeat;
    public int BackTo;
    public List<int> frames = new List<int>();
    public AnimationDirection(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {
        //if (Settings.GameType == GameType.TwoFivePlus) Console.WriteLine("current position: " + Reader.Tell() + " out of size " + Reader.Size());
        long currentPosition = Reader.Tell();

        MinSpeed = Reader.ReadSByte();
        MaxSpeed = Reader.ReadSByte();
        Repeat = Reader.ReadInt16();
        BackTo = Reader.ReadInt16();
        var frameCount = Reader.ReadUInt16();
        if (frameCount > 250) //idk
        {
            Console.WriteLine("Invalid amount of frames, skipping");
            return;
        }
        for (int i = 0; i < frameCount; i++)
        {

            var handle = Reader.ReadInt16();
            //if (Settings.GameType == GameType.TwoFivePlus) Console.WriteLine("adding image #" + i);
            frames.Add(handle);


        }


    }


}
#endif