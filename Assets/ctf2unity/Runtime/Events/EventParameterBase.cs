using Ctf2Unity.Runtime.Events.ParameterTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events
{
    public abstract class EventParameterBase : ScriptableObject
    {
#if UNITY_EDITOR
        public static EventParameterBase CreateParam(short code, ParameterCommon par)
        {
            Type type = null;
            switch (code)
            {
                case 1:
                    type = typeof(ObjectInfoParam);
                    break;

                case 2:
                    type = typeof(TimeParam);
                    break;

                case 5:
                case 25:
                case 29:
                case 34:
                case 48:
                case 56:
                    type = typeof(IntParam);
                    break;

                case 14:
                case 44:
                    type = typeof(KeyParam);
                    break;

                case 16:
                    type = typeof(PositionParam);
                    break;

                case 15:
                case 22:
                case 23:
                    type = typeof(ExpressionParam);
                    break;
            }
            if (type == null)
            {
                Debug.LogError($"Missing param: {code}");
                return null;
            }

            var ins = (EventParameterBase)CreateInstance(type);
            ins.Create(par);
            return ins;
        }

        public abstract void Create(ParameterCommon par);
#endif
    }
}
