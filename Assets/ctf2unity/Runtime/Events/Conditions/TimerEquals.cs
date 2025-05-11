using Ctf2Unity.Runtime;
using Ctf2Unity.Runtime.Events;
using Ctf2Unity.Runtime.Events.ParameterTypes;
using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class TimerEquals : CheckConditionBase
{
    public override bool Check()
    {
        var par = parameters[0] as TimeParam;
        Debug.Log($"{par.ms}:{FrameHandler.current.frameTimer}");
        return par.ms == (int)FrameHandler.current.frameTimer;
    }
}

