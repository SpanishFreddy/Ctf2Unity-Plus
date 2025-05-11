using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events
{
    [Serializable]
    public abstract class EventActionBase : ScriptableObject
    {
        [NonSerialized] public EventContext context;
        public List<EventParameterBase> parameters = new List<EventParameterBase>();

        public abstract void Execute();

        public virtual void Initialize() { }

        public static EventActionBase CreateAction(int objectType, int num)
        {
            switch (objectType)
            {
                case -3:
                    switch (num)
                    {
                        case 0:
                            return CreateInstance<NextFrame>();

                        case 1:
                            return CreateInstance<PreviousFrame>();

                        case 4:
                            return CreateInstance<EndTheApplication>();

                        case 5:
                            return CreateInstance<RestartTheApplication>();

                        case 6:
                            return CreateInstance<RestartCurrentFrame>();
                    }
                    break;

                case 2:
                    switch (num)
                    {
                        case 1:
                            return CreateInstance<SetPosition>();

                        case 2:
                            return CreateInstance<SetXPos>();

                        case 3:
                            return CreateInstance<SetYPos>();

                        case 24:
                            return CreateInstance<DestroyObject>();

                        case 26:
                            return CreateInstance<MakeInvisible>();

                        case 27:
                            return CreateInstance<MakeReappear>();
                    }
                    break;
                case 7:
                    switch(num)
                    {
                        case 80:
                            return CreateInstance<SetCounterValue>();
                        case 81:
                            return CreateInstance<AddToCounter>();
                        case 24:
                            return CreateInstance<DestroyObject>();

                    }
                    break;

            }
            Debug.LogError($"Missing action: [Type: {objectType}; Code: {num}]");
            return null;
        }
    }
}
