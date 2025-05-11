using Ctf2Unity.Runtime.InstanceObjectTypes;
using Ctf2Unity.Runtime.ObjectInfoTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events.ParameterTypes
{
    [Serializable]
    public class ExpressionParam : EventParameterBase
    {
        [SerializeField] private int[] OTs;
        [SerializeField] private CommonObjectInfoBase[] objectInfos;
        [SerializeField] private int[] Codes;
        [SerializeField] private string[] stringLitarals;
        [SerializeField] private double[] numberLiterals;
        [SerializeField] private char[] mask;

        private EventContext context;
        private int index;

        public string GetStringValue()
        {
            return "TestCock";
        }

        public double GetDoubleValue(EventContext context, int index = 0)
        {
            this.context = context;
            this.index = index;
            int i = 0;
            return Calculate(ref i);
        }

        private double Calculate(ref int i)
        {
            double value = 0;
            int nextOperation = 2;
            var len = mask.Length;
            for (; i < len; i++)
            {
                switch (mask[i])
                {
                    case 'c':
                        switch (OTs[i])
                        {
                            case -1:
                                switch (Codes[i])
                                {
                                    case -2:
                                        return value;

                                    case -1:
                                        i++;
                                        UpdateValue(ref value, nextOperation, Calculate(ref i));
                                        break;
                                }
                                break;

                            case 0:
                                nextOperation = Codes[i];
                                break;

                            case 2:
                                UpdateValue(ref value, nextOperation, GetRetrievableValue(Codes[i], objectInfos[i]));
                                break;

                        }
                        break;

                    case 'd':
                        UpdateValue(ref value, nextOperation, numberLiterals[i]);
                        break;
                }
            }

            return value;
        }

        private double GetRetrievableValue(int code, CommonObjectInfoBase objectInfo)
        {
            var cont = objectInfo == null ? null : context.GetObjects(objectInfo);
            CommonObjectBase ins = objectInfo == null ? null : (cont == null || cont.Count == 0 ? (objectInfo.Instances.Count == 0 ? null : objectInfo.Instances[index % objectInfo.Instances.Count]) : cont[0]);
            if (objectInfo != null && ins == null)
                return 0;

            switch (code)
            {
                case 1:
                    return ins.PixelPosition.y;

                case 11:
                    return ins.PixelPosition.x;
            }

            return 0;
        }

        private static void UpdateValue(ref double value, int op, double calcValue)
        {
            switch (op)
            {
                case 2:
                    value += calcValue;
                    break;

                case 4:
                    value -= calcValue;
                    break;

                case 6:
                    value *= calcValue;
                    break;

                case 8:
                    value /= calcValue;
                    break;

                case 12:
                    value = Math.Pow(value, calcValue);
                    break;
            }
        }

#if UNITY_EDITOR
        public override void Create(ParameterCommon par)
        {
            if(par is ExpressionParameter exp)
            {
                Codes = new int[exp.Items.Count];
                mask = new char[exp.Items.Count];
                OTs = new int[exp.Items.Count];
                objectInfos = new CommonObjectInfoBase[exp.Items.Count];
                numberLiterals = new double[exp.Items.Count];
                stringLitarals = new string[exp.Items.Count];
                for (int i = 0; i < Codes.Length; i++)
                {
                    var item = exp.Items[i];
                    if(item.Loader==null)
                    {
                        Codes[i] = item.Num;
                        OTs[i] = item.ObjectType;
                        mask[i] = 'c';
                        var inf = FrameHandler.current.GetObjectInfoByHandle(item.ObjectInfo);
                        if (inf is CommonObjectInfoBase com)
                            objectInfos[i] = com;
                    }
                    else
                    {
                        if(item.Loader is LongExp longE)
                        {
                            numberLiterals[i] = (int)longE.Value;
                            mask[i] = 'd';
                        }
                        else if(item.Loader is DoubleExp doubleE)
                        {
                            numberLiterals[i] = (double)doubleE.Value;
                            mask[i] = 'd';
                        }
                    }

                }
            }

        }
#endif
    }
}
