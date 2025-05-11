#if UNITY_EDITOR
public class StringParam : ParameterCommon
{
    public string Value;

    public StringParam(ByteReader reader) : base(reader)
    {

    }

    public override void Read()
    {
        Value = Reader.ReadAscii();
    }


}
#endif