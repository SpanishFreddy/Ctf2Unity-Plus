using Ctf2Unity.Runtime.Events.ParameterTypes;
using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events.Conditions
{
    [Serializable]
    public class RepeatWhilePressed : CheckConditionBase
    {
        public KeyParam Param => (KeyParam)parameters[0];

        public override bool Check()
        {
            return Input.GetKey(Param.key);
        }
    }
}
