using Ctf2Unity.Runtime;
using Ctf2Unity.Runtime.Events;
using Ctf2Unity.Runtime.Events.ParameterTypes;
using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SetPosition : CommonObjectActionBase
{
    public PositionParam Param => (PositionParam)parameters[0];

    public override void Execute(CommonObjectBase obj, int index)
    {
        var pos = Param.GetPosition(index);
        if (!pos.HasValue)
            return;

        obj.transform.position = pos.Value;
    }
}
