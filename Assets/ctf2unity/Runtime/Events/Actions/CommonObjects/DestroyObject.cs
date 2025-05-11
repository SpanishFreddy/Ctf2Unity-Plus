using Ctf2Unity.Runtime;
using Ctf2Unity.Runtime.Events;
using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DestroyObject : CommonObjectActionBase
{
    public override void Execute(CommonObjectBase obj, int index)
    {
        Destroy(obj.gameObject);
    }
}