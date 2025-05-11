#if UNITY_EDITOR
using UnityEngine;

class Colour : ParameterCommon
    {
        public Color32 Value;

        public Colour(ByteReader reader) : base(reader) { }
        public override void Read()
        {
            var bytes = Reader.ReadBytes(4);
            Value = new Color32(bytes[0],bytes[1],bytes[2],bytes[3]);
        }


    }
#endif
