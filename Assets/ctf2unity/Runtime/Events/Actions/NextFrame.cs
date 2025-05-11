using Ctf2Unity.Runtime;
using Ctf2Unity.Runtime.Events;
using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NextFrame : EventActionBase
{
    public override void Execute()
    {
        FrameHandler.current.NextFrame();
    }
}
