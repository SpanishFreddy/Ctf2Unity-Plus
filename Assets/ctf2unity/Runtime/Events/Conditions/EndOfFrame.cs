using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events.Conditions
{
    [Serializable]
    public class EndOfFrame : EventConditionBase
    {
        public override void Initialize()
        {
            FrameHandler.current.onEnd.AddListener(Invoke);
        }
    }
}
