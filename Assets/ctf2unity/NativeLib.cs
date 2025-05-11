using System;
using System.Runtime.InteropServices;
    public static class NativeLib
    {

        private const string _dllPath = "Decrypter-x64.dll";


        [DllImport(_dllPath, EntryPoint = "decode_chunk", CharSet = CharSet.Auto)]
        public static extern IntPtr decode_chunk(IntPtr chunkData, int chunkSize, byte magicChar, IntPtr wrapperKey);

        [DllImport(_dllPath, EntryPoint = "make_key", CharSet = CharSet.Auto)]
        public static extern IntPtr make_key(IntPtr cTitle, IntPtr cCopyright, IntPtr cProject, byte magicChar);

        [DllImport(_dllPath, EntryPoint = "make_key_w", CharSet = CharSet.Unicode)]
        public static extern IntPtr make_key_w([MarshalAs(UnmanagedType.LPTStr)]string cTitle, [MarshalAs(UnmanagedType.LPTStr)] string cCopyright, [MarshalAs(UnmanagedType.LPTStr)] string cProject, byte magicChar);

        [DllImport(_dllPath, EntryPoint = "make_key_combined", CharSet = CharSet.Auto)]
        public static extern IntPtr make_key_combined(IntPtr data, byte magicChar);

        [DllImport(_dllPath, EntryPoint = "make_key_w_combined", CharSet = CharSet.Auto)]
        public static extern IntPtr make_key_w_combined(IntPtr data, byte magicChar);

        [DllImport(_dllPath, EntryPoint = "GenChecksum", CharSet = CharSet.Auto)]
        public static extern UInt32 GenChecksum(IntPtr name, IntPtr pass);

        [DllImport(_dllPath, EntryPoint = "decompressOld", CharSet = CharSet.Auto)]
        public static extern int decompressOld(IntPtr source, int source_size, IntPtr output, int output_size);

    }