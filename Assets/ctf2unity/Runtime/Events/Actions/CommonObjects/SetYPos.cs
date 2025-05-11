using Ctf2Unity.Runtime;
using Ctf2Unity.Runtime.Events;
using Ctf2Unity.Runtime.Events.ParameterTypes;
using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SetYPos : CommonObjectActionBase
{
    public ExpressionParam Param => (ExpressionParam)parameters[0];

    public override void Execute(CommonObjectBase obj, int index)
    {
        var t = obj.transform;
        t.position = new Vector2(t.position.x, (float)Param.GetDoubleValue(context, index) / -100f);
    }
}
