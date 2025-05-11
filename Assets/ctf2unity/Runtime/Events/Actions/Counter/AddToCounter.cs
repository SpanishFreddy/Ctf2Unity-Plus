using Ctf2Unity.Runtime.Events;
using Ctf2Unity.Runtime.Events.ParameterTypes;
using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class AddToCounter : CommonObjectActionBase
{
    public override void Execute(CommonObjectBase obj, int index)
    {
        var value = (ExpressionParam)parameters[0];
        var cntr = (CounterObject)obj;
        cntr.Value = cntr.Value+value.GetDoubleValue(context, index);
    }
}

