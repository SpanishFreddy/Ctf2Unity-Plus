using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events.Conditions.Handlers
{
    [Serializable]
    public class OneActionOnLoop : ConditionsHandlerBase
    {
        private bool wasMet;

        public override bool HandleResult(bool conditionsMet)
        {
            if (conditionsMet)
            {
                if (wasMet)
                    return false;
                else
                {
                    wasMet = true;
                    return true;
                }
            }
            else
            {
                wasMet = false;
                return true;
            }
        }

        public override bool HandleBeforeCheck()
        {
            return true;
        }

        public override bool HandleClearContextObjects()
        {
            return !wasMet;
        }
    }
}
