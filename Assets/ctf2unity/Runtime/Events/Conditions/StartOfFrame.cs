using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events.Conditions
{
    [Serializable]
    public class StartOfFrame : EventConditionBase
    {
        public override void Initialize()
        {
            FrameHandler.current.onStart.AddListener(Invoke);
        }
    }
}
