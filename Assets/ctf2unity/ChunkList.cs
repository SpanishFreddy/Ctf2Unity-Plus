#if UNITY_EDITOR
using Ctf2Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;


public class ChunkList
{
    public List<Chunk> Chunks = new List<Chunk>();
    public bool Verbose = false;


    public void Read(ByteReader reader)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        Chunks.Clear();

        while (true)
        {
            Chunk chunk = new Chunk(Chunks.Count);
            chunk.Read(reader);
            chunk.Loader = LoadChunk(chunk);

            if (chunk.Loader != null) chunk.Loader.Chunk = chunk;

            Chunks.Add(chunk);
            if (reader.Tell() >= reader.Size()) break; //In case there is no LAST chunk(custom protection maybe)
            if (chunk.Id == 32639) break; //LAST chunkID
            //if (chunk.Id == 8750) BuildKey(); //Only build key when we have all the needed info


        }
        stopwatch.Stop();
        //Logger.Log("Data Left: "+(reader.Size()-reader.Tell()).ToPrettySize());
    }

    public class Chunk
    {
        public string Name
        {
            get => "sample text";
        }
        int _uid;
        public int Id = 0;

        public ChunkLoader Loader;
        public byte[] ChunkData;
        public byte[] RawData;
        public ChunkFlags Flag;
        public int Size;
        public int DecompressedSize = -1;
        public bool Verbose = false;

        public Chunk(int actualuid)
        {
            _uid = actualuid;
        }

        public ByteReader GetReader()
        {
            return new ByteReader(ChunkData);
        }
        public static byte[] LoadChunkData(byte[] data, ChunkFlags flag,short id)
        {
            switch (flag)
            {
                case ChunkFlags.Encrypted:
                    return Decryption.DecryptChunk(data, data.Length);
                    break;
                case ChunkFlags.CompressedAndEncrypted:
                    return Decryption.DecodeMode3(data, data.Length, id, out _);
                    //Console.WriteLine(ChunkData.Length);
                    break;
                case ChunkFlags.Compressed:
                    return Decompressor.Decompress(new ByteReader(data), out _);
                    break;
                case ChunkFlags.NotCompressed:
                    return data;
                    break;
                default:
                    return null;
            }
        }

        public void Read(ByteReader exeReader)
        {
            Id = exeReader.ReadInt16();
            Flag = (ChunkFlags)exeReader.ReadInt16();
            Size = exeReader.ReadInt32();

            //if (Id == 8754 || Id == 13108 || Id == 8759 || Id == 8747 || Id == 13122) Flag = ChunkFlags.NotCompressed;
            //Console.WriteLine("***ChunkList: reading " + (Constants.ChunkNames)Id + " (" + Size + ") - " + Flag);

            switch (Flag)
            {
                case ChunkFlags.Encrypted:
                    ChunkData = Decryption.DecryptChunk(exeReader.ReadBytes(Size), Size);
                    break;
                case ChunkFlags.CompressedAndEncrypted:
                    ChunkData = Decryption.DecodeMode3(exeReader.ReadBytes(Size), Size, Id, out DecompressedSize);
                    //Console.WriteLine(ChunkData.Length);
                    break;
                case ChunkFlags.Compressed:
                    ChunkData = Decompressor.Decompress(exeReader, out DecompressedSize);
                    break;
                case ChunkFlags.NotCompressed:
                    ChunkData = exeReader.ReadBytes(Size);
                    break;
            }
            //Console.WriteLine("***Chunk size: " + ChunkData.Length);
            if (ChunkData == null) throw new NullReferenceException("ChunkData is null after reading");
            //Save();
        }






    }

    public void BuildKey()
    {
        Ctf2UnityProcessor.Log($"Generating {Settings.Build>284} key");
        var AppName = GetChunk<AppName>()?.Value ?? "";
        var Copyright = GetChunk<Copyright>()?.Value ?? "";
        var ProjectPath = GetChunk<EditorFilename>()?.Value ?? "";
        if (Settings.Build > 284) Decryption.MakeKey(AppName, Copyright, ProjectPath);
        else Decryption.MakeKey(ProjectPath, AppName, Copyright);
        Ctf2UnityProcessor.Log("Key generated");
    }

    public enum ChunkFlags
    {
        NotCompressed = 0,
        Compressed = 1,
        Encrypted = 2,
        CompressedAndEncrypted = 3
    }



    public ChunkLoader LoadChunk(Chunk chunk)
    {
        var reader = chunk.GetReader();
        ChunkLoader loader = null;
        //Console.WriteLine("reading chunk " + chunk.Id);
        switch (chunk.Id)
        {
            case 8739:
                loader = new AppHeader(reader);
                break;
            case 8740:
                loader = new AppName(reader);
                break;
            case 8741:
                loader = new AppAuthor(reader);
                break;
            case 8742:
                loader = new AppMenu(reader);
                break;
            case 8743:
                loader = new ExtPath(reader);
                break;
            case 8747:
                loader = new FrameHandles(reader);
                break;
            case 8750:
                loader = new EditorFilename(reader);
                break;
            case 8751:
                loader = new TargetFilename(reader);
                break;
            case 8752:
                loader = new AppDoc(reader);
                break;
            case 8771:
                loader = new Shaders(reader);
                break;
            case 8756:
                loader = new Extensions(reader);
                break;
            case 8745:
                loader = new FrameItems(reader);
                break;
            case 8762:
                loader = new AboutText(reader);
                break;
            case 8763:
                loader = new Copyright(reader);
                break;
            case 13123:
                loader = new DemoFilePath(reader);
                break;
            case 13109:
                loader = new FrameName(reader);
                break;
            case 13107:
                loader = new Frame(reader);
                break;
            case 13108:
                loader = new FrameHeader(reader);
                break;
            case 13111:
                loader = new FramePalette(reader);
                break;
            case 13112:
                loader = new ObjectInstances(reader);
                break;
            case 13115:
                loader = new Transition(reader);
                break;
            case 13116:
                loader = new Transition(reader);
                break;
            case 13122:
                loader = new VirtualRect(reader);
                break;
            case 13121:
                loader = new Layers(reader);
                break;
            case 26214:
                loader = new ImageBank(reader);
                break;
            case 26216:
                if (Settings.GameType == GameType.Android) break;
                loader = new SoundBank(reader);
                break;
            case 26217:
                if (Settings.GameType == GameType.Android) break;
                loader = new MusicBank(reader);
                break;
            case 26215:
                loader = new FontBank(reader);
                break;
            case 17477:
                loader = new ObjectName(reader);
                break;
            case 17476:
                loader = new ObjectHeader(reader);
                break;
            case 8748:
                loader = new ExtData(reader);
                break;
            case 17478:
                loader = new ObjectProperties(reader);
                return loader;

            case 8754:
                loader = new GlobalValues(reader);
                break;
            case 8755:
                loader = new GlobalStrings(reader);
                break;
            case 13117:
                loader = new Events(reader);
                break;
            case 13127:
                loader = new MovementTimerBase(reader);
                break;

            default:
                Console.WriteLine("(unknown chunk " + chunk.Id + ")");
                break;
        }

        if (loader != null)
            Ctf2UnityProcessor.Log(loader.GetType().Name);
        loader?.Read();
        // chunk.ChunkData = null; //TODO:Do something smarter
        // chunk.RawData = null;
        return loader;
    }



    public T GetChunk<T>() where T : ChunkLoader
    {
        foreach (Chunk chunk in Chunks)
        {
            if (chunk.Loader != null)
            {
                if (chunk.Loader.GetType() == typeof(T))
                {
                    return (T)chunk.Loader;
                }
            }
        }

        //Logger.Log($"ChunkLoader {typeof(T).Name} not found", true, ConsoleColor.Red);
        return null;
    }

    public T PopChunk<T>() where T : ChunkLoader
    {
        for (int i = 0; i < Chunks.Count; i++)
        {
            var chunk = Chunks[i];
            if (chunk.Loader != null)
            {
                if (chunk.Loader is T loaderT)
                {
                    Chunks.Remove(chunk);
                    return loaderT;
                }
            }
        }

        return null;
    }
}
#endif