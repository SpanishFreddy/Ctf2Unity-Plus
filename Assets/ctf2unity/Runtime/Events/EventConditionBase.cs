using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Ctf2Unity.Runtime.Events
{
    [Serializable]
    public abstract class EventConditionBase : ConditionBase
    {
        public UnityEvent onEvent = new UnityEvent();

        public abstract void Initialize();

        public void Invoke()
        {
            onEvent.Invoke();
        }
    }
}
