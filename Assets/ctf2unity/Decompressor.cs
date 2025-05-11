using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using Ionic.Zlib;


    public static class Decompressor
    {
        public static byte[] Decompress(ByteReader exeReader, out int decompressed)
        {
            Int32 decompSize = exeReader.ReadInt32();
            Int32 compSize = exeReader.ReadInt32();
            decompressed = decompSize;
            return DecompressBlock(exeReader, compSize, decompSize);
        }

        public static ByteReader DecompressAsReader(ByteReader exeReader, out int decompressed) =>
            new ByteReader(Decompress(exeReader, out decompressed));


        public static byte[] DecompressBlock(ByteReader reader, int size, int decompSize=0)
        {
            

            return ZlibStream.UncompressBuffer(reader.ReadBytes(size));
        }



        





        public static byte[] compress_block(byte[] data)
        {

            return ZlibStream.CompressBuffer(data);
        }
    }
