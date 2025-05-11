using Ctf2Unity.Runtime.Events.ParameterTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events.Conditions
{
    [Serializable]
    public class EveryTimespan : EventConditionBase
    {
        public float Seconds
        {
            get
            {
                var par = parameters[0];
                if (par is TimeParam time)
                {
                    return time.Seconds;
                }

                return (float)((ExpressionParam)par).GetDoubleValue(null);
            }
        }

        public override void Initialize()
        {
            FrameHandler.current.StartCoroutine(Run());
        }

        public IEnumerator Run()
        {
            for (; ; )
            {
                yield return new WaitForSeconds(Seconds);
                Invoke();
            }
        }
    }
}
