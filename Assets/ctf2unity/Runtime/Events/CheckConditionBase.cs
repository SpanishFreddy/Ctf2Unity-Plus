using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ctf2Unity.Runtime.Events
{
    [Serializable]
    public abstract class CheckConditionBase : ConditionBase
    {
        public abstract bool Check();
    }
}
