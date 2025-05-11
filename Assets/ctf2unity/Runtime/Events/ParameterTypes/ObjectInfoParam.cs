using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ctf2Unity.Runtime.Events.ParameterTypes
{
    public class ObjectInfoParam : EventParameterBase
    {
        public int objectHandle;

#if UNITY_EDITOR
        public override void Create(ParameterCommon par)
        {

        }
#endif
    }
}
