using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events.ParameterTypes
{
    public class IntParam : EventParameterBase
    {
        public int value;

#if UNITY_EDITOR
        public override void Create(ParameterCommon par)
        {
            var ind = (Int)par;
            value = ind.Value;
        }
#endif
    }
}
