using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ctf2Unity.Runtime.Events.Conditions.CommonObjects
{
    public class IsInvisible : CommonObjectConditionBase
    {
        public override bool Check(CommonObjectBase obj, int index) => obj.Visibility == VisibilityState.Invisible;
    }
}
