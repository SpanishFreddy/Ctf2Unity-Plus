#define UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using static ChunkList;

#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;
using UnityEditor;
using UnityEngine;

namespace Ctf2Unity
{
    public class Ctf2UnityProcessor
    {
        public readonly ByteReader reader;
        public AppName Name;
        public AppAuthor Author;
        public Copyright Copyright;
        public string AboutText;
        public string Doc;
        public string version = "1.0.0.0";

        public EditorFilename EditorFilename;
        public TargetFilename TargetFilename;

        //public ExeOnly Exe_Only;

        public AppMenu Menu;

        public AppHeader Header;
        //public ExtentedHeader ExtHeader;

        public FontBank Fonts;
        public SoundBank Sounds;
        public MusicBank Music;
        public ImageBank Images;

        public GlobalValues GValues;
        public GlobalStrings GStrings;
        public static FrameItems TestItems;

        //public Extensions Ext;

        public FrameItems Frameitems;

        public List<Frame> frames = new List<Frame>();
        public FrameHandles FrameHandles;
        public Extensions extensions;

        public static StreamWriter logStream;

        public Ctf2UnityProcessor(string ctfFilePath)
        {
            var ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(ctfFilePath);
            version = ver.ProductVersion;
            reader = new ByteReader(File.OpenRead(ctfFilePath));
        }

        public Ctf2UnityProcessor(Stream ctfStream)
        {
            reader = new ByteReader(ctfStream);
        }

        public Ctf2UnityProcessor(byte[] ctfRaw)
        {
            reader = new ByteReader(new MemoryStream(ctfRaw));
        }

        public static void Log(string log)
        {
            if (logStream == null)
                return;

            logStream.WriteLine(DateTime.Now.ToString("[H:mm:ss.fff] ") + log);
        }

        public void Start()
        {
            var assets = Application.dataPath;
            logStream = File.CreateText(Path.Combine(assets.Substring(0, assets.Length - 6), "Logs\\Ctf2Unity " + DateTime.Now.Ticks + ".log"));
            logStream.AutoFlush = true;

            ReadGameData(reader);

            AppInfo.Create(this);

            for (int a = 0; a < 31; a++)
            {
                Physics2D.SetLayerCollisionMask(a, 1 << a);
            }

            Directory.CreateDirectory(Path.Combine(Application.dataPath, "Frames"));
            EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[frames.Count];
            for (int a = 0; a < frames.Count; a++)
            {
                var gen = new FrameGenerator(this);
                gen.Generate(frames[a]);
                scenes[a] = new EditorBuildSettingsScene(gen.assetPath, true);
            }
            EditorBuildSettings.scenes = scenes;

            logStream.Dispose();
            logStream = null;
        }

        public void ReadGameData(ByteReader reader)
        {
            var sig = reader.ReadAscii(2);
            if (sig != "MZ") throw new InvalidDataException("Invalid MZ header");

            reader.Seek(60);

            var hdrOffset = reader.ReadUInt16();


            reader.Seek(hdrOffset);
            var peHdr = reader.ReadAscii(2);
            if (peHdr != "PE") throw new InvalidDataException("Invalid PE header");
            reader.Skip(4);

            var numOfSections = reader.ReadUInt16();

            reader.Skip(16);
            var optionalHeader = 28 + 68;
            var dataDir = 16 * 8;
            reader.Skip(optionalHeader + dataDir);

            var possition = 0;
            for (var i = 0; i < numOfSections; i++)
            {
                var entry = reader.Tell();

                var sectionName = reader.ReadAscii();

                if (sectionName == ".extra")
                {
                    reader.Seek(entry + 20);
                    possition = (int)reader.ReadUInt32(); //Pointer to raw data
                    break;
                }

                if (i >= numOfSections - 1)
                {
                    reader.Seek(entry + 16);
                    var size = reader.ReadUInt32();
                    var address = reader.ReadUInt32(); //Pointer to raw data

                    possition = (int)(address + size);
                    break;
                }

                reader.Seek(entry + 40);
            }

            reader.Seek(possition);

            var firstShort = reader.PeekInt16();
            if (firstShort == 0x7777)
            {
                //we are at the correct position, we can parse the pack data now
                long start = reader.Tell();
                var _header = reader.ReadBytes(8);

                // reader.Skip(8);
                uint headerSize = reader.ReadUInt32();
                Debug.Assert(headerSize == 32);
                uint dataSize = reader.ReadUInt32();

                reader.Seek((int)(start + dataSize - 32));
                var uheader = reader.ReadAscii(4);

                reader.Seek(start + 16);

                var formatVersion = reader.ReadUInt32();
                var check = reader.ReadInt32();
                Debug.Assert(check == 0);
                check = reader.ReadInt32();
                Debug.Assert(check == 0);

                uint count = reader.ReadUInt32();


                long offset = reader.Tell();
                for (int i = 0; i < count; i++)
                {
                    if (!reader.Check(2)) break;
                    ushort value = reader.ReadUInt16();
                    if (!reader.Check(value)) break;
                    reader.ReadBytes(value);
                    reader.Skip(value);
                    if (!reader.Check(value)) break;
                }

                var newHeader = reader.ReadAscii(4);
                bool hasBingo = newHeader != "PAME" && newHeader != "PAMU";

                reader.Seek(offset);
                for (int i = 0; i < count; i++)
                {
                    // read all the pack files
                    ushort len = reader.ReadUInt16();
                    var PackFilename = reader.ReadWideString(len);
                    var _bingo = reader.ReadInt32();
                    var size = reader.ReadInt32();
                    reader.Skip(size);//skipping the packdata files because i dont care about extensions

                }
                //PackData is done, moving to the GameData
                string magic = reader.ReadAscii(4); //Reading header

                var RuntimeVersion = (short)reader.ReadUInt16();
                var RuntimeSubversion = (short)reader.ReadUInt16();
                var ProductVersion = reader.ReadInt32();
                var ProductBuild = reader.ReadInt32();
                Settings.Build = ProductBuild;
                Debug.Log($"Product version: " + ProductBuild);
                while (true)
                {
                    var chunkId = reader.ReadInt16();
                    var chunkFlag = (ChunkFlags)reader.ReadInt16();
                    var chunkSize = reader.ReadInt32();
                    if (chunkId == 32639) break;
                    byte[] chunkData = Chunk.LoadChunkData(reader.ReadBytes(chunkSize),chunkFlag,chunkId);
                    if (chunkData == null) throw new InvalidDataException("Invalid ChunkData");
                    var chunkReader = new ByteReader(chunkData);
                    switch(chunkId)
                    {
                        case 8739:
                            Header = new AppHeader(chunkReader);
                            Header.Read();
                            break;
                        case 8740:
                            Name = new AppName(chunkReader);
                            Name.Read();
                            break;
                        case 8741:
                            Author = new AppAuthor(chunkReader);
                            Author.Read();
                            break;
                        case 8742:
                            Menu = new AppMenu(chunkReader);
                            Menu.Read();
                            break;
                        case 8747:
                            FrameHandles = new FrameHandles(chunkReader);
                            FrameHandles.Read();
                            break;
                        case 8750:
                            EditorFilename = new EditorFilename(chunkReader);
                            EditorFilename.Read();

                            if (Settings.Build > 284) Decryption.MakeKey(Name ?? "", Copyright ?? "", EditorFilename ?? "");
                            else Decryption.MakeKey(EditorFilename ?? "", Name ?? "", Copyright ?? "");
                            break;
                        case 8751:
                            TargetFilename = new TargetFilename(chunkReader);
                            TargetFilename.Read();
                            break;
                        case 8756:
                            extensions = new Extensions(chunkReader);
                            extensions.Read();
                            break;
                        case 8745:
                            Frameitems = new FrameItems(chunkReader);
                            Frameitems.Read();
                            break;
                        case 8763:
                            Copyright = new Copyright(chunkReader);
                            Copyright.Read();
                            break;
                        case 13107:
                            var newFrame = new Frame(chunkReader);
                            newFrame.Read();
                            frames.Add(newFrame);
                            break;
                        case 26214:
                            Images = new ImageBank(chunkReader);
                            Images.Read();
                            break;
                        case 26216:
                            Sounds = new SoundBank(chunkReader);
                            Sounds.Read();
                            break;
                        case 26217:
                            Music = new MusicBank(chunkReader);
                            Music.Read();
                            break;
                        case 8754:
                             GValues = new GlobalValues(chunkReader);
                            GValues.Read();
                            break;
                    }
                    chunkReader.Close();

                }
                /*var GameChunks = new ChunkList();
                GameChunks.Read(reader);

                Log("Popping Chunks");
                Name = GameChunks.PopChunk<AppName>();
                Copyright = GameChunks.PopChunk<Copyright>();
                Author = GameChunks.PopChunk<AppAuthor>();
                EditorFilename = GameChunks.PopChunk<EditorFilename>();
                TargetFilename = GameChunks.PopChunk<TargetFilename>();
                Menu = GameChunks.PopChunk<AppMenu>();
                Header = GameChunks.PopChunk<AppHeader>();
                Sounds = GameChunks.PopChunk<SoundBank>();
                Music = GameChunks.PopChunk<MusicBank>();
                Fonts = GameChunks.PopChunk<FontBank>();
                Images = GameChunks.PopChunk<ImageBank>();
                GValues = GameChunks.PopChunk<GlobalValues>();
                FrameHandles = GameChunks.PopChunk<FrameHandles>();
                extensions = GameChunks.PopChunk<Extensions>();
                Frameitems = GameChunks.PopChunk<FrameItems>();

                for (int i = 0; i < Header.NumberOfFrames; i++)
                {
                    frames.Add(GameChunks.PopChunk<Frame>());
                }*/

                reader.Close();
            }
        }
    }
}
#endif