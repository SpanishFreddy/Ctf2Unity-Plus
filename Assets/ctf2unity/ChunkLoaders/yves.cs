#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Drawing;


namespace CTFAK.MMFParser.EXE.Loaders
{
    public class AppIcon : ChunkLoader
    {
        public AppIcon(ByteReader reader) : base(reader)
        {
        }



        public override void Read()
        {

        }

    }
}
#endif