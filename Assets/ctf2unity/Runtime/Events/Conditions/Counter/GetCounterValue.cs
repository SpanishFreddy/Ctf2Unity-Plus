using Ctf2Unity.Runtime.Events.ParameterTypes;
using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ctf2Unity.Runtime.Events.Conditions.Counter
{
    public class GetCounterValue : CommonObjectConditionBase
    {
        public override bool Check(CommonObjectBase obj, int index) => ((CounterObject)obj).Value==((ExpressionParam)parameters[0]).GetDoubleValue(context,index);
    }
}
