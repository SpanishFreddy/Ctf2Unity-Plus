using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events.Conditions
{
    [Serializable]
    public class IsVSyncEnabled : CheckConditionBase
    {
        public override bool Check() => QualitySettings.vSyncCount != 0;
    }
}
