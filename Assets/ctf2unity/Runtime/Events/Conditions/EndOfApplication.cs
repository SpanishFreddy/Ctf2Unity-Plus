using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events.Conditions
{
    [Serializable]
    public class EndOfApplication : EventConditionBase
    {
        public override void Initialize()
        {
            Application.quitting += Invoke;
        }
    }
}
