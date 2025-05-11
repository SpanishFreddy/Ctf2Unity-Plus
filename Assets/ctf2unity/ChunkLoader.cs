#if UNITY_EDITOR
using static ChunkList;

public abstract class ChunkLoader
{
    public Chunk Chunk;
    public ByteReader Reader;
    public ChunkLoader(ByteReader reader)
    {
        Reader = reader;
    }
    public abstract void Read();

}
#endif