using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ctf2Unity.Runtime.Events
{
    [Serializable]
    public abstract class ConditionsHandlerBase : ConditionBase
    {
        [NonSerialized] public ConditionsGroup conditionsGroup;

        public abstract bool HandleResult(bool conditionsMet);
        public abstract bool HandleBeforeCheck();
        public abstract bool HandleClearContextObjects();
    }
}
