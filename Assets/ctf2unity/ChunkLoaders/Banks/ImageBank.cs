#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;
using UnityEditor;
using UnityEngine;

public class ImageBank : ChunkLoader
{
    public List<Image> images;

    public ImageBank(ByteReader reader) : base(reader)
    {
        Image.root = Application.dataPath + "\\Resources\\Textures\\";
        Directory.CreateDirectory(Image.root);
    }

    public Sprite GetImageSprite(int handle)
    {
        for (int a = 0; a < images.Count; a++)
        {
            var img = images[a];
            if (img.Handle == handle)
                return img.finalSprite;
        }
        return null;
    }

    // Start is called before the first frame update
    public override void Read()
    {
        //var files = Directory.GetFiles(Image.root, "*.png");
        //var len = files.Length;
        //images = new List<Image>();
        //for (int a = 0; a < len; a++)
        //{
        //    var file = files[a];
        //    images.Add(new Image("Assets/Resources/Textures/" + Path.GetFileName(file)));
        //}
        //return;

        var count = Reader.ReadInt32();
        images = new List<Image>();
        for (int i = 0; i < count; i++)
        {
            var img = new Image(Reader);
            img.Read();
            images.Add(img);
        }

        AssetDatabase.Refresh();

        for (int a = 0; a < images.Count; a++)
        {
            var img = images[a];
            var asset = "Assets/Resources/Textures/" + img.Handle.ToString() + ".png";
            var imp = (TextureImporter)AssetImporter.GetAtPath(asset);

            imp.textureType = TextureImporterType.Sprite;
            imp.isReadable = true;
            imp.spritePivot = new Vector2(img.hotspot.x / (float)img.width, 1f - img.hotspot.y / (float)img.height);
            imp.filterMode = FilterMode.Bilinear;

            var maxSize = imp.maxTextureSize;
            if (img.width > maxSize || img.height > maxSize)
            {
                imp.maxTextureSize = Mathf.Max(img.width, img.height);
            }

            var settings = new TextureImporterSettings();
            imp.ReadTextureSettings(settings);
            settings.spriteAlignment = (int)SpriteAlignment.Custom;
            imp.SetTextureSettings(settings);

            imp.SaveAndReimport();

            img.finalSprite = AssetDatabase.LoadAssetAtPath<Sprite>(asset);
        }
    }

}
public class Image : ChunkLoader
{
    public static string root;

    public int Handle;
    int Position;
    int _checksum;
    int _references;
    public int width;
    public int height;
    int _graphicMode;
    public Vector2Int hotspot;
    public Vector2Int action;

    BitDict Flags = new BitDict(new string[]
    {
            "RLE",
            "RLEW",
            "RLET",
            "LZX",
            "Alpha",
            "ACE",
            "Mac"
    });

    public int Size;

    Color32 _transparent;

    public Sprite finalSprite;
    public string path;

    public bool debug = false;
    public Image(ByteReader reader) : base(reader) { }
    public Image(string file) : base(new ByteReader(Array.Empty<byte>()))
    {
        finalSprite = AssetDatabase.LoadAssetAtPath<Sprite>(file);
        Handle = int.Parse(Path.GetFileNameWithoutExtension(file));
    }

    public void CleanUp()
    {
        Flags = null;
    }

    public override void Read()
    {
        Handle = Reader.ReadInt32();
        if (Settings.Build >= 284) Handle -= 1;
        Int32 decompSize = Reader.ReadInt32();
        Int32 compSize = Reader.ReadInt32();
        var compressedBuffer = Reader.ReadBytes(compSize);
        var uncompressedData = ZlibStream.UncompressBuffer(compressedBuffer);
        var imageReader = new ByteReader(uncompressedData);
        _checksum = imageReader.ReadInt32();
        _references = imageReader.ReadInt32();
        
        Size = (int)imageReader.ReadUInt32();
        width = imageReader.ReadInt16();
        height = imageReader.ReadInt16();
        _graphicMode = imageReader.ReadByte();
        Flags.flag = imageReader.ReadByte();
        imageReader.Skip(2);
        hotspot.x = imageReader.ReadInt16();
        hotspot.y = imageReader.ReadInt16();
        action.x = imageReader.ReadInt16();
        action.y = imageReader.ReadInt16();
        //if (_references == 1)
        //{
        //    hotspot.x = 0;
        //    hotspot.y = 0;
        //    action.x = 0;
        //    action.y = 0;
        //}
            _transparent = imageReader.ReadColor();
        byte[] imageData;
        if (Flags["LZX"])
        {
            uint decompressedSize = imageReader.ReadUInt32();

            imageData = ZlibStream.UncompressBuffer(imageReader.ReadBytes((int)(Reader.Size() - Reader.Tell())));
        }
        else imageData = imageReader.ReadBytes((int)(Size));
        Load(imageData);
        imageData = null;
        CleanUp();
    }

    public void Load(byte[] imgData)
    {
        int position = 0;
        byte[] colorArray = new byte[width * height * 4];
        int stride = 0;
        int pad = 0;
        switch (_graphicMode)
        {
            case 4:
                stride = width * 4;
                pad = GetPadding(width, 3);
                for (int y = height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < width; x++)
                    {
                        colorArray[(y * stride) + (x * 4) + 0] = imgData[position];
                        colorArray[(y * stride) + (x * 4) + 1] = imgData[position + 1];
                        colorArray[(y * stride) + (x * 4) + 2] = imgData[position + 2];
                        colorArray[(y * stride) + (x * 4) + 3] = 255;
                        position += 3;
                    }

                    position += pad * 3; //the pad is usually 1
                }
                break;
            case 6:
                stride = width * 4;
                pad = GetPadding(width, 2);
                for (int y = height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < width; x++)
                    {
                        UInt16 newShort = (ushort)(imgData[position] | imgData[position + 1] << 8);
                        byte r = (byte)((newShort & 31744) >> 10);
                        byte g = (byte)((newShort & 992) >> 5);
                        byte b = (byte)((newShort & 31));

                        r = (byte)(r << 3);
                        g = (byte)(g << 3);
                        b = (byte)(b << 3);
                        colorArray[(y * stride) + (x * 4) + 2] = r;
                        colorArray[(y * stride) + (x * 4) + 1] = g;
                        colorArray[(y * stride) + (x * 4) + 0] = b;
                        colorArray[(y * stride) + (x * 4) + 3] = 255;
                        position += 2;
                    }

                    position += pad * 2;
                }
                break;
            case 7:
                stride = width * 4;
                pad = GetPadding(width, 2);
                for (int y = height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < width; x++)
                    {
                        UInt16 newShort = (ushort)(imgData[position] | imgData[position + 1] << 8);
                        byte r = (byte)((newShort & 63488) >> 11);
                        byte g = (byte)((newShort & 2016) >> 5);
                        byte b = (byte)((newShort & 31));

                        r = (byte)(r << 3);
                        g = (byte)(g << 2);
                        b = (byte)(b << 3);
                        colorArray[(y * stride) + (x * 4) + 2] = r;
                        colorArray[(y * stride) + (x * 4) + 1] = g;
                        colorArray[(y * stride) + (x * 4) + 0] = b;
                        colorArray[(y * stride) + (x * 4) + 3] = 255;
                        position += 2;
                    }

                    position += pad * 2;
                }
                break;
            
        }
        
        
        var alphaSize = Size - position;

        if (Flags["Alpha"])
        {
            int alphaPad = GetPadding(width, 1, 4);
            byte[,] alpha = new byte[width, height];
            var possition = Size - alphaSize;
            for (int i = height - 1; i >= 0; i--)
            {
                for (int j = 0; j < width; j++)
                {
                    alpha[j, i] = imgData[possition];
                    possition += 1;
                }

                possition += alphaPad;
            }
            int alphaStride = width * 4;
            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    colorArray[(y * alphaStride) + (x * 4) + 3] = alpha[x, y];
                }
            }
        }
        if (!Flags["Alpha"])
        {
            for (int i = 0; i < (height * width *4); i+=4)
            {
                if (colorArray[i] == _transparent.r && colorArray[i + 1] == _transparent.g &&
                    colorArray[i + 2] == _transparent.b)
                {
                    colorArray[i+3] =0;
                }
            }
        }
        var texture = new Texture2D(width, height, TextureFormat.BGRA32, false);
        texture.LoadRawTextureData(colorArray);
        texture.Apply();

        path = root + Handle.ToString() + ".png";
        Ctf2Unity.Ctf2UnityProcessor.Log("Saving image to: " + path);
        File.WriteAllBytes(path, texture.EncodeToPNG());
        AssetDatabase.ImportAsset("Assets/Resources/Textures/" + Handle.ToString() + ".png");
    }

    public static int GetPadding(int width, int pointSize, int bytes = 2)
    {
        int pad = bytes - ((width * pointSize) % bytes);
        if (pad == bytes)
        {
            return 0;
        }

        return (int)Math.Ceiling((double)((float)pad / (float)pointSize));
    }

    public static T[] To1DArray<T>(T[,] input)
    {
        // Step 1: get total size of 2D array, and allocate 1D array.
        int size = input.Length;
        T[] result = new T[size];

        // Step 2: copy 2D array elements into a 1D array.
        int write = 0;
        for (int i = 0; i <= input.GetUpperBound(0); i++)
        {
            for (int z = 0; z <= input.GetUpperBound(1); z++)
            {
                result[write++] = input[i, z];
            }
        }

        // Step 3: return the new array.
        return result;
    }
}
#endif