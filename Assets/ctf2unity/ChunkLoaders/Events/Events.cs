#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Events : ChunkLoader
{
    public readonly string Header = "ER>>";
    public readonly string EventCount = "ERes";
    public readonly string EventgroupData = "ERev";
    public readonly string End = "<<ER";
    public readonly string ExtensionData = "ERop";

    public int MaxObjects;
    public int MaxObjectInfo;
    public int NumberOfPlayers;
    public Dictionary<int, Quailifer> QualifiersList = new Dictionary<int, Quailifer>();
    public List<int> NumberOfConditions = new List<int>();
    public List<EventGroup> Items = new List<EventGroup>();





    public override void Read()
    {
        // if (Settings.GameType == GameType.OnePointFive) return;
        while (true)
        {

            var identifier = Reader.ReadAscii(4);
            //Console.WriteLine("event ID: " + identifier);
            if (identifier == Header)
            {
                MaxObjects = Reader.ReadInt16();
                MaxObjectInfo = Reader.ReadInt16();
                NumberOfPlayers = Reader.ReadInt16();
                for (int i = 0; i < 17; i++)
                {
                    NumberOfConditions.Add(Reader.ReadInt16());
                }

                var qualifierCount = Reader.ReadInt16(); //should be 0, so i dont care
                for (int i = 0; i < qualifierCount; i++)
                {
                    var newQualifier = new Quailifer(Reader);
                    newQualifier.Read();
                    if (!QualifiersList.ContainsKey(newQualifier.ObjectInfo)) QualifiersList.Add(newQualifier.ObjectInfo, newQualifier);
                }
                //Console.WriteLine("event header: max objects " + MaxObjects + "," + MaxObjectInfo + " number of players " + NumberOfPlayers + " number of conditions " + NumberOfConditions.Sum() + " number of qualifiers " + qualifierCount);
            }
            else if (identifier == EventCount)
            {
                var size = Reader.ReadInt32();
                //Console.WriteLine("event count: there are " + size + " events"); //this is incorrect - the event size is not the same as the actual number of lines of events
            }
            else if (identifier == ExtensionData)
            {
                //Console.WriteLine("reading ERop");
                var size = Reader.ReadInt32();
                //Console.WriteLine("extension data (ERop): the size is " + size);
            }
            else if (identifier == EventgroupData)
            {
                var size = Reader.ReadInt32();
                var endPosition = Reader.Tell() + size;
                //Console.WriteLine("event group: size " + size + "/" + endPosition);
                //if (Settings.GameType == GameType.TwoFivePlus) Reader.Seek(endPosition);
                while (true)
                {
                    //Console.WriteLine("making eventgroup reader");
                    var eg = new EventGroup(Reader);
                    //Console.WriteLine("reading eventgroup data");
                    eg.Read();
                    //Console.WriteLine("adding eventgroup items");
                    Items.Add(eg);
                    //Console.WriteLine("done");
                    if (Reader.Tell() >= endPosition) break;
                }

            }
            else if (identifier == End || identifier == "  <<") break;
        }
    }

    public Events(ByteReader reader) : base(reader)
    {
    }
    public static ChunkLoader LoadParameter(int code, ByteReader reader)
    {
        ChunkLoader item = null;
        if (code >= 68 || code == 0)
        {
            //Console.WriteLine("Illegal 2.5+ instruction encountered: " + code);
        }
        if (code == 1)
        {
            item = new ParamObject(reader);
        }

        if (code == 2)
        {
            item = new Time(reader);
        }
        if (code == 3 || code == 4 || code == 10 || code == 11 || code == 12 || code == 17 || code == 26 || code == 31 ||
            code == 43 || code == 57 || code == 58 || code == 60 || code == 61)
        {
            item = new Short(reader);
        }
        if (code == 5 || code == 25 || code == 29 || code == 34 || code == 48 || code == 56)
        {
            item = new Int(reader);
        }
        if (code == 6 || code == 7 || code == 35 || code == 36)
        {
            item = new Sample(reader);
        }
        if (code == 9 || code == 21)
        {
            item = new Create(reader);
        }
        if (code == 13)
        {
            item = new Every(reader);
        }
        if (code == 14 || code == 44)
        {
            item = new KeyParameter(reader);
        }
        if (code == 15 || code == 22 || code == 23 || code == 27 || code == 28 || code == 45 || code == 46 || code == 52 || code == 53 || code == 54 || code == 59 || code == 62)
        {
            item = new ExpressionParameter(reader);
        }
        if (code == 16)
        {
            item = new Position(reader);
        }
        if (code == 18)
        {
            item = new Shoot(reader);
        }
        if (code == 19)
        {
            item = new Zone(reader);
        }
        if (code == 24)
        {
            item = new Colour(reader);
        }

        if (code == 40)
        {
            item = new Filename(reader);
        }

        if (code == 32)
        {
            item = new Click(reader);
        }

        if (code == 33)
        {
            item = new Program(reader);
        }

        if (code == 55)
        {
            item = new Extension(reader);
        }

        if (code == 38)
        {
            item = new Group(reader);
        }

        if (code == 39)
        {
            item = new GroupPointer(reader);
        }

        if (code == 49)
        {
            item = new GlobalValue(reader);
        }

        if (code == 41 || code == 64)
        {
            item = new StringParam(reader);
        }

        if (code == 47 || code == 51)
        {
            item = new TwoShorts(reader);
        }

        return item;
    }
}

public class Quailifer : ChunkLoader
{
    public int ObjectInfo;
    public int Type;
    public int Qualifier;
    List<int> _objects = new List<int>();



    public Quailifer(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {
        ObjectInfo = Reader.ReadUInt16();
        Type = Reader.ReadInt16();
        Qualifier = ObjectInfo & 0b11111111111;
    }


}


public class EventGroup : ChunkLoader
{
    public ushort Flags;
    public int IsRestricted;
    public int RestrictCpt;
    public int Identifier;
    public int Undo;
    public List<Condition> Conditions = new List<Condition>();
    public List<Action> Actions = new List<Action>();
    public int Size;
    public byte NumberOfConditions;
    public byte NumberOfActions;
    public bool isMFA = false;



    public EventGroup(ByteReader reader) : base(reader)
    {
    }



    public override void Read()
    {
        var currentPosition = Reader.Tell();
        Size = Reader.ReadInt16() * -1;
        NumberOfConditions = Reader.ReadByte();
        NumberOfActions = Reader.ReadByte();
        Flags = Reader.ReadUInt16();

        if (Settings.Build >= 284) //&&(Settings.GameType == GameType.Normal)
        {
            var nop = Reader.ReadInt16();
            IsRestricted = Reader.ReadInt32();
            RestrictCpt = Reader.ReadInt32();
        }
        else
        {
            IsRestricted = Reader.ReadInt16();
            RestrictCpt = Reader.ReadInt16();
            Identifier = Reader.ReadInt16();
            Undo = Reader.ReadInt16();
        }


        //Console.WriteLine("---Cond: {"+NumberOfConditions+"},Act: {"+NumberOfActions+"}");
        for (int i = 0; i < NumberOfConditions; i++)
        {
            //Console.WriteLine("-Reading condition #" + i);
            var item = new Condition(Reader);
            item.Read();
            //Console.WriteLine("-Adding condition #" + i);
            Conditions.Add(item);
            //Console.WriteLine("done adding condition #" + i);
        }

        for (int i = 0; i < NumberOfActions; i++)
        {
            //Console.WriteLine("-Reading action #" + i);
            var item = new Action(Reader);
            item.Read();
            //Console.WriteLine("-Adding action #" + i);
            Actions.Add(item);
            //Console.WriteLine("done adding action #" + i);
        }
        Reader.Seek(currentPosition + Size);
        // Logger.Log($"COND:{NumberOfConditions}, ACT: {NumberOfActions}");

    }


}

public static class Fixer
{
    public static void FixConditions(ref Condition cond)
    {
        var num = cond.Num;
        //Alterable Values:
        if (num == -42) num = -27;
        //Global Values
        //if (num == -28||num == -29||num == -30||num == -31||num == -32||num == -33) num = -8;
        cond.Num = num;
    }
    public static void FixActions(ref Action act)
    {
        var num = act.Num;
        act.Num = num;
    }


}
#endif