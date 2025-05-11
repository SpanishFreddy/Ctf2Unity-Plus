#if UNITY_EDITOR
using System.Collections.Generic;

public class ExpressionParameter : ParameterCommon
{
    public List<Expression> Items;
    public short Comparsion;

    public ExpressionParameter(ByteReader reader) : base(reader)
    {
    }

    public override void Read()
    {
        Comparsion = Reader.ReadInt16();
        Items = new List<Expression>();
        while (true)
        {
            var expression = new Expression(Reader);
            expression.Read();
            //Console.WriteLine($"Found expression {expression.ObjectType}-{expression.Num}=={((ExpressionLoader)expression.Loader)?.Value}");
            if (expression.ObjectType == 0&&expression.Num==0)
            {
                break;
            }
            else
            {
                Items.Add(expression);
                //Console.WriteLine("Adding expression");
            }

            // if(expression.Num==23||expression.Num==24||expression.Num==50||expression.Num==16||expression.Num==19)Logger.Log("CUMSHOT "+expression.Num);

        }
    }



    public string GetOperator()
    {
        switch (Comparsion)
        {
            case 0: return "==";
            case 1: return "!=";
            case 2: return "<=";
            case 3: return "<";
            case 4: return ">=";
            case 5: return ">";
            default: return "err";
        }
    }


}
#endif