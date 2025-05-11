#if UNITY_EDITOR
using System;

public class StringChunk : ChunkLoader
{
    public string Value="ERRORNULL";


    public override void Read()
    {
        Value = Reader.ReadWideString();
    }



    public override string ToString()
    {
        return Value;
    }
    public static implicit operator string(StringChunk chunk)
    {
        return chunk.Value;
    }


    public StringChunk(ByteReader reader) : base(reader)
    {
    }


}

public class AppName : StringChunk
{
    public AppName(ByteReader reader) : base(reader)
    {
    }


}

public class AppAuthor : StringChunk
{
    public AppAuthor(ByteReader reader) : base(reader)
    {
    }


}

class ExtPath : StringChunk
{
    public ExtPath(ByteReader reader) : base(reader)
    {
    }


}

public class EditorFilename : StringChunk
{
    public EditorFilename(ByteReader reader) : base(reader)
    {
    }


}

public class TargetFilename : StringChunk
{
    public TargetFilename(ByteReader reader) : base(reader)
    {
    }

}

class AppDoc : StringChunk
{
    public AppDoc(ByteReader reader) : base(reader)
    {
    }


}

class AboutText : StringChunk
{
    public AboutText(ByteReader reader) : base(reader)
    {
    }


}

public class Copyright : StringChunk
{
    public Copyright(ByteReader reader) : base(reader)
    {
    }


}

class DemoFilePath : StringChunk
{
    public DemoFilePath(ByteReader reader) : base(reader)
    {
    }


}
#endif