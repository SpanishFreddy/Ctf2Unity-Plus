using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events.ParameterTypes
{
    public class TimeParam : EventParameterBase
    {
        public int ms;
        public float Seconds => ms / 1000f;

#if UNITY_EDITOR
        public override void Create(ParameterCommon par)
        {
            var time = (Time)par;
            ms = time.Timer;
        }
#endif
    }
}
