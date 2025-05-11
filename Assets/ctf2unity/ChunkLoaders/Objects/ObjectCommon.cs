#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using UnityEngine;

public class ObjectCommon : ChunkLoader
{
    private ushort _valuesOffset;
    private ushort _stringsOffset;
    private uint _fadeinOffset;
    private uint _fadeoutOffset;
    private ushort _movementsOffset;
    private ushort _animationsOffset;
    private ushort _systemObjectOffset;
    private ushort _counterOffset;
    private ushort _extensionOffset;
    public string Identifier;
    public Animations Animations;

    [Flags]
    public enum ObjectCommonPrefs
    {
        Backsave = 1,
        ScrollingIndependant = 2,
        QuickDisplay = 4,
        Sleep = 8,
        LoadOnCall = 16,
        Global = 32,
        BackEffects = 64,
        Kill = 128,
        InkEffects = 256,
        Transitions = 512,
        FineCollisions = 1024,
        AppletProblems = 2048,
    }

    public ObjectCommonPrefs Preferences;
    [Flags]
    public enum ObjectCommonFlags
    {
        DisplayInFront = 1,
        Background = 2,
        Backsave = 4,
        RunBeforeFadeIn = 8,
        Movements = 16,
        Animations = 32,
        TabStop = 64,
        WindowProc = 128,
        Values = 512,
        Sprites = 1024,
        InternalBacksave = 2048,
        ScrollingIndependant = 4096,
        QuickDisplay = 8192,
        NeverKill = 16384,
        NeverSleep = 32768,
        ManualSleep = 65536,
        Text = 131072,
        DoNotCreateAtStart = 262144,
        FakeSprite = 524288,
        FakeCollisions = 1048576,
    }
    public ObjectCommonFlags Flags;

    [Flags]
    public enum ObjectCommonNewFlags
    {
        DoNotSaveBackground = 1,
        SolidBackground = 2,
        CollisionBox = 4,
        VisibleAtStart = 8,
        ObstacleSolid = 16,
        ObstaclePlatform = 32,
        AutomaticRotation = 64
    }
    public ObjectCommonNewFlags NewFlags;


    public Color32 BackColor;
    public ObjectInfoReader Parent;
    public Counters Counters;
    public byte[] ExtensionData;
    public int ExtensionPrivate;
    public int ExtensionId;
    public int ExtensionVersion;
    public AlterableValues Values;
    public AlterableStrings Strings;
    public Movements Movements;
    public Text Text;
    public Counter Counter;
    public short[] _qualifiers = new short[8];

    //twoFilePlusOnly
    public bool isFirstRead = true;
    public int twoFilePlusPos;
    public ByteReader decompressedReader;


    public ObjectCommon(ByteReader reader) : base(reader)
    {
    }
    public ObjectCommon(ByteReader reader, ObjectInfoReader parent) : base(reader)
    {
        Parent = parent;
    }



    public override void Read()
    {
        {
            var currentPosition = Reader.Tell();
            twoFilePlusPos = (int)currentPosition;
            //Console.WriteLine("is about to read the object " + isFirstRead + " at position " + currentPosition + "/" + Reader.Size());
            //if (currentPosition >= Reader.Size()) return; //this can't be in for some reason or there will be no images
            //if (currentPosition > 25800) isFirstRead = false;
            if (Settings.Build >= 284)//new no 1.5
            {
                var size = Reader.ReadInt32();
                _animationsOffset = Reader.ReadUInt16();
                _movementsOffset = Reader.ReadUInt16();
                var version = Reader.ReadUInt16();
                Reader.Skip(2);
                _extensionOffset = Reader.ReadUInt16();
                _counterOffset = Reader.ReadUInt16();
                Flags = (ObjectCommonFlags)Reader.ReadInt32();
                var end = Reader.Tell() + 8 * 2;
                for (int i = 0; i < 8; i++)
                {
                    _qualifiers[i] = Reader.ReadInt16();
                }

                Reader.Seek(end);

                _systemObjectOffset = Reader.ReadUInt16();

                _valuesOffset = Reader.ReadUInt16();
                _stringsOffset = Reader.ReadUInt16();
                NewFlags = (ObjectCommonNewFlags)Reader.ReadUInt16();
                Preferences = (ObjectCommonPrefs)Reader.ReadUInt16();
                Identifier = Reader.ReadAscii(4);
                BackColor = Reader.ReadColor();
                _fadeinOffset = Reader.ReadUInt32();
                _fadeoutOffset = Reader.ReadUInt32();
            }
            else
            {
                var size = Reader.ReadInt32();
                _movementsOffset = Reader.ReadUInt16();
                _animationsOffset = Reader.ReadUInt16();
                var version = Reader.ReadUInt16();
                _counterOffset = Reader.ReadUInt16();
                _systemObjectOffset = Reader.ReadUInt16();
                Reader.Skip(2);
                Flags = (ObjectCommonFlags)Reader.ReadUInt32();
                // Reader.Skip(2);
                var end = Reader.Tell() + 8 * 2;
                for (int i = 0; i < 8; i++)
                {
                    _qualifiers[i] = Reader.ReadInt16();
                }

                Reader.Seek(end);

                _extensionOffset = Reader.ReadUInt16();

                _valuesOffset = Reader.ReadUInt16();
                _stringsOffset = Reader.ReadUInt16();
                NewFlags = (ObjectCommonNewFlags)Reader.ReadUInt16();
                Preferences = (ObjectCommonPrefs)Reader.ReadUInt16();
                Identifier = Reader.ReadAscii(2);
                BackColor = Reader.ReadColor();
                _fadeinOffset = Reader.ReadUInt32();
                _fadeoutOffset = Reader.ReadUInt32();
            }

            //if (Settings.GameType == GameType.TwoFivePlus) return;

            //we finished reading the offsets
            //Console.WriteLine("Movement  Offset: " + _movementsOffset);
            //Console.WriteLine("Values    Offset: " + _valuesOffset);
            //Console.WriteLine("Counter   Offset: " + _counterOffset);
            //Console.WriteLine("Sys Obj   Offset: " + _systemObjectOffset);
            //Console.WriteLine("Extension Offset: " + _extensionOffset);
            //Console.WriteLine("Animation Offset: " + _animationsOffset);
            //Console.WriteLine("Strings   Offset: " + _stringsOffset);


            if (_animationsOffset > 0)
            {
                Reader.Seek(currentPosition + _animationsOffset);
                Animations = new Animations(Reader);
                Animations.Read();
            }


            if (_movementsOffset > 0)
            {
                Reader.Seek(currentPosition + _movementsOffset);
                Movements = new Movements(Reader);
                Movements.Read();

            }

            if (_systemObjectOffset > 0)
            {

                Reader.Seek(currentPosition + _systemObjectOffset);
                switch (Parent.ObjectType)
                {
                    //Text
                    case Constants.ObjectType.Text:
                        Text = new Text(Reader);
                        Text.Read();
                        break;
                    //Counter
                    case Constants.ObjectType.Counter:
                    case Constants.ObjectType.Score:
                    case Constants.ObjectType.Lives:
                        Counters = new Counters(Reader);
                        Counters.Read();
                        break;

                }
            }

            if (_extensionOffset > 0)
            {

                Reader.Seek(currentPosition + _extensionOffset);

                var dataSize = Reader.ReadInt32() - 20;
                Reader.Skip(4); //maxSize;
                ExtensionVersion = Reader.ReadInt32();
                ExtensionId = Reader.ReadInt32();
                ExtensionPrivate = Reader.ReadInt32();
                if (dataSize != 0)
                {
                    ExtensionData = Reader.ReadBytes(dataSize);
                }
                else ExtensionData = new byte[0];

            }

            if (_counterOffset > 0)
            {
                Reader.Seek(currentPosition + _counterOffset);
                Counter = new Counter(Reader);
                Counter.Read();
            }



        }
    }
}
#endif



