#if UNITY_EDITOR
using Ctf2Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;


static class Decryption
{
    private static byte[] _decryptionKey;
    public static byte MagicChar = 99;
    //public static byte MagicChar = 54;


    public static void MakeKey(string data1, string data2, string data3)
    {
        _decryptionKey = null;
        Ctf2UnityProcessor.Log("Entered MakeKey Method");
        // MakeKeyUnicode(data1,data2,data3);
        // return;
        IntPtr keyPtr;
        var combined = "";
        combined += data1;
        combined += data2;
        combined += data3;
        UnityEngine.Debug.Log("Combined data " + combined);
        var data1Ptr = Marshal.StringToHGlobalAnsi(data1);
        var data2Ptr = Marshal.StringToHGlobalAnsi(data2);
        var data3Ptr = Marshal.StringToHGlobalAnsi(data3);
        Ctf2UnityProcessor.Log("Pointers created");

        //keyPtr = NativeLib.make_key(data1Ptr,data2Ptr,data3Ptr, MagicChar);
        keyPtr = NativeLib.make_key(data1Ptr, data2Ptr, data3Ptr, MagicChar);
        Ctf2UnityProcessor.Log("Native func called");
        byte[] key = new byte[256];
        Marshal.Copy(keyPtr, key, 0, 256);
        Ctf2UnityProcessor.Log("Key copied");
        //Marshal.FreeHGlobal(keyPtr);
        Marshal.FreeHGlobal(data1Ptr);
        Marshal.FreeHGlobal(data2Ptr);
        Marshal.FreeHGlobal(data3Ptr);
        Ctf2UnityProcessor.Log("Memory freed");
        _decryptionKey = key;
    }






    public static byte[] DecodeMode3(byte[] chunkData, int chunkSize, int chunkId, out int decompressed)
    {
        ByteReader reader = new ByteReader(chunkData);
        uint decompressedSize = reader.ReadUInt32();
        byte[] rawData = reader.ReadBytes((int)reader.Size());
        if ((chunkId & 1) == 1 && Settings.Build > 284)
        {
            rawData[0] ^= (byte)((byte)(chunkId & 0xFF) ^ (byte)(chunkId >> 0x8));
        }
        int rawSize = rawData.Length;
        rawData = DecryptChunk(rawData, chunkSize);
        using (ByteReader data = new ByteReader(rawData))
        {
            uint compressedSize = data.ReadUInt32();
            //Console.WriteLine($"!!!Decrypting chunk {chunkId}: original size {chunkSize} decompressed size {decompressedSize} actual size {rawSize} decrypted size {rawData.Length} actual compressed {compressedSize}");
            decompressed = (int)decompressedSize;
            return Decompressor.DecompressBlock(data, (int)compressedSize, (int)decompressedSize);
        }
    }


    public static byte[] DecryptChunk(byte[] chunkData, int chunkSize)
    {
        IntPtr inputChunkPtr = Marshal.AllocHGlobal(chunkData.Length);
        Marshal.Copy(chunkData, 0, inputChunkPtr, chunkData.Length);

        IntPtr keyPtr = Marshal.AllocHGlobal(_decryptionKey.Length);
        Marshal.Copy(_decryptionKey, 0, keyPtr, _decryptionKey.Length);

        var outputChunkPtr = NativeLib.decode_chunk(inputChunkPtr, chunkSize, MagicChar, keyPtr);

        byte[] decodedChunk = new byte[chunkSize];
        Marshal.Copy(outputChunkPtr, decodedChunk, 0, chunkSize);

        Marshal.FreeHGlobal(inputChunkPtr);
        Marshal.FreeHGlobal(keyPtr);

        return decodedChunk;
    }





}
#endif