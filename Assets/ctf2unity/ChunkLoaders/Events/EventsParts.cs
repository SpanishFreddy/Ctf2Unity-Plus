#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

    public class Condition : ChunkLoader
    {
        public int Flags;
        public int OtherFlags;
        public int DefType;
        public int NumberOfParameters;
        public int ObjectType;
        public int Num;
        public int ObjectInfo;
        public int Identifier;
        public int ObjectInfoList;
        public List<Parameter> Items = new List<Parameter>();

        public Condition(ByteReader reader) : base(reader) { }


        public override void Read()
        {

            var currentPosition = Reader.Tell();
            var size = Reader.ReadUInt16();

            ObjectType =  Reader.ReadInt16();
            Num = Reader.ReadInt16();

            ObjectInfo = Reader.ReadUInt16();
            ObjectInfoList = Reader.ReadInt16();
            Flags = Reader.ReadSByte();
            OtherFlags = Reader.ReadSByte();
            NumberOfParameters = Reader.ReadByte();
            DefType = Reader.ReadByte();
            Identifier = Reader.ReadInt16();
            for (int i = 0; i < NumberOfParameters; i++)
            {
                var item = new Parameter(Reader);
                item.Read();
                Items.Add(item);
            }
            Reader.Seek(currentPosition + size);
            



    }
    public override string ToString()
        {
            //return Preprocessor.ProcessCondition(this);
            //return $"Condition {(Constants.ObjectType)ObjectType}=={Names.ConditionNames[ObjectType][Num]}{(Items.Count > 0 ? "-"+Items[0].ToString() : " ")}";
            return $"Condition {(Constants.ObjectType)ObjectType}=={Num}{(Items.Count > 0 ? "-"+Items[0].ToString() : " ")}";
        }
    }

    public class Action : ChunkLoader
    {
        public int Flags;
        public int OtherFlags;
        public int DefType;
        public int ObjectType;
        public int Num;
        public int ObjectInfo;
        public int ObjectInfoList;
        public List<Parameter> Items = new List<Parameter>();
        public byte NumberOfParameters;
        public Action(ByteReader reader) : base(reader) { }
        

        public override void Read()
        {

            var old = false;
            var currentPosition = Reader.Tell();
            var size = Reader.ReadUInt16();
            ObjectType =  old ? Reader.ReadSByte(): Reader.ReadInt16();
            Num = old ? Reader.ReadSByte(): Reader.ReadInt16();
            if ((int) ObjectType >= 2 && Num >= 48)
            {
                if(old)Num += 32;
            }
            ObjectInfo = Reader.ReadUInt16();
            ObjectInfoList = Reader.ReadInt16();
            Flags = Reader.ReadSByte();
            OtherFlags = Reader.ReadSByte();
            NumberOfParameters = Reader.ReadByte();
            DefType = Reader.ReadByte();
            for (int i = 0; i < NumberOfParameters; i++)
            {
                var item = new Parameter(Reader);
                item.Read();
                Items.Add(item);
            }
        //Logger.Log(this);
        Reader.Seek(currentPosition + size);

        }
        public override string ToString()
        {
            
            return $"Action {ObjectType}-{Num}{(Items.Count > 0 ? "-"+Items[0].ToString() : " ")}";

        }
    }

    public class Parameter : ChunkLoader
    {
        public short Code;
        public ChunkLoader Loader;

        public Parameter(ByteReader reader) : base(reader) { }




        public override void Read()
        {

            var currentPosition = Reader.Tell();
            var size = Reader.ReadInt16();
            Code = Reader.ReadInt16();

            ChunkLoader actualLoader = Events.LoadParameter(Code,Reader);
            this.Loader = actualLoader;
            if (Loader != null) Loader.Read();

            Reader.Seek(currentPosition+size);

        }
        public object Value
        {
            get
            {
                if (Loader != null)
                {


                    if (Loader.GetType().GetField("value") != null)
                    {
                        return Loader.GetType().GetField("value").GetValue(Loader);
                    }
                    else
                    {
                        return null;
                    }
                }
                else return null;
            }
        }
        public override string ToString()
        {
            if (Loader != null) return Loader.ToString();
            else return "String: ERROR!"; 
            //else throw new Exception($"Unkown Parameter: {Code} ");
        }
    }
#endif