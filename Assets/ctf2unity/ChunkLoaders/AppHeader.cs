#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using UnityEngine;

public class AppHeader : ChunkLoader
{
    public int Size;
    public int WindowWidth;
    public int WindowHeight;
    public int InitialScore;
    public int InitialLives;
    public int NumberOfFrames;
    public BitDict Flags = new BitDict(new string[]
    {
            "BorderMax",
            "NoHeading",
            "Panic",
            "SpeedIndependent",
            "Stretch",
            "MusicOn",
            "SoundOn",
            "MenuHidden",
            "MenuBar",
            "Maximize",
            "MultiSamples",
            "FullscreenAtStart",
            "FullscreenSwitch",
            "Protected",
            "Copyright",
            "OneFile"
    });
    public BitDict NewFlags = new BitDict(new string[]
    {
            "SamplesOverFrames",
            "RelocFiles",
            "RunFrame",
            "SamplesWhenNotFocused",
            "NoMinimizeBox",
            "NoMaximizeBox",
            "NoThickFrame",
            "DoNotCenterFrame",
            "ScreensaverAutostop",
            "DisableClose",
            "HiddenAtStart",
            "XPVisualThemes",
            "VSync",
            "RunWhenMinimized",
            "MDI",
            "RunWhileResizing"
    });

    public Color32 BorderColor;
    public int FrameRate;
    public short GraphicsMode;
    public short Otherflags;
    public Controls Controls;
    public int WindowsMenuIndex;


    public override void Read()
    {

        {
            var start = Reader.Tell();
            Size = Reader.ReadInt32();
            Flags.flag = (uint)Reader.ReadInt16();

            NewFlags.flag = (uint)Reader.ReadInt16();
            GraphicsMode = Reader.ReadInt16();
            Otherflags = Reader.ReadInt16();
            WindowWidth = Reader.ReadInt16();
            WindowHeight = Reader.ReadInt16();
            InitialScore = (int)(Reader.ReadUInt32() ^ 0xffffffff);
            InitialLives = (int)(Reader.ReadUInt32() ^ 0xffffffff);
            Controls = new Controls(Reader);

            // if (Settings.GameType == GameType.OnePointFive) Reader.Skip(56);
            // else Controls.Read();
            Controls.Read();
            BorderColor = Reader.ReadColor();
            NumberOfFrames = Reader.ReadInt32();
            FrameRate = Reader.ReadInt32();
            WindowsMenuIndex = Reader.ReadInt32();
        }


    }




    public AppHeader(ByteReader reader) : base(reader)
    {
    }


}


public class Controls : ChunkLoader
{
    public List<PlayerControl> Items;

    public Controls(ByteReader reader) : base(reader)
    {
        this.Reader = reader;
    }



    public override void Read()
    {
        Items = new List<PlayerControl>();
        for (int i = 0; i < 4; i++)
        {
            var item = new PlayerControl(Reader);
            Items.Add(item);
            item.Read();
        }
    }


}

public class PlayerControl
{
    int _controlType;
    ByteReader _reader;
    Keys _keys;

    public PlayerControl(ByteReader reader)
    {
        this._reader = reader;
    }

    public void Read()
    {
        _keys = new Keys(_reader);
        _controlType = _reader.ReadInt16();
        _keys.Read();
    }

}

public class Keys
{
    short _up;
    short _down;
    short _left;
    short _right;
    short _button1;
    short _button2;
    short _button3;
    short _button4;
    ByteReader _reader;

    public Keys(ByteReader reader)
    {
        this._reader = reader;
    }


    public void Read()
    {
        _up = _reader.ReadInt16();
        _down = _reader.ReadInt16();
        _left = _reader.ReadInt16();
        _right = _reader.ReadInt16();
        _button1 = _reader.ReadInt16();
        _button2 = _reader.ReadInt16();
        _button3 = _reader.ReadInt16();
        _button4 = _reader.ReadInt16();
    }




}
#endif