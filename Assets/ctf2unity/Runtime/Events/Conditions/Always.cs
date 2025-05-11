using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events.Conditions
{
    [Serializable]
    public class Always : CheckConditionBase
    {
        public override bool Check() => true;
    }
}
