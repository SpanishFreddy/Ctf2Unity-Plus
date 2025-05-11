using Ctf2Unity.Runtime.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ctf2unity.Runtime.Events.ParameterTypes
{
    class ShortParam:EventParameterBase
    {
        public short value;

#if UNITY_EDITOR
        public override void Create(ParameterCommon par)
        {
            var ind = (Short)par;
            value = ind.Value;
        }
#endif
    }
}
