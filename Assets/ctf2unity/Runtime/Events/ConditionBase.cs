using Ctf2Unity.Runtime.Events.Conditions;
using Ctf2Unity.Runtime.Events.Conditions.CommonObjects;
using Ctf2Unity.Runtime.Events.Conditions.Counter;
using Ctf2Unity.Runtime.Events.Conditions.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events
{
    [Serializable]
    public abstract class ConditionBase : ScriptableObject
    {
        [NonSerialized] public EventContext context;
        public List<EventParameterBase> parameters = new List<EventParameterBase>();

        public static ConditionBase CreateCondition(int objectType, int num)
        {
            switch (objectType)
            {
                case 2:
                    switch (num)
                    {
                        case -28:
                            return CreateInstance<IsInvisible>();

                        case -29:
                            return CreateInstance<IsVisible>();
                    }
                    break;

                case -1:
                    switch (num)
                    {
                        case -1:
                            return CreateInstance<Always>();

                        case -2:
                            return CreateInstance<Never>();

                        case -7:
                            return CreateInstance<OneActionOnLoop>();

                        case -24:
                            return CreateInstance<ORFiltered>();

                        case -25:
                            return CreateInstance<ORFiltered>();
                    }
                    break;

                case -3:
                    switch (num)
                    {
                        case -1:
                            return CreateInstance<StartOfFrame>();

                        case -2:
                            return CreateInstance<EndOfFrame>();

                        case -4:
                            return CreateInstance<EndOfApplication>();

                        case -7:
                            return CreateInstance<IsVSyncEnabled>();
                    }
                    break;

                case -4:
                    switch (num)
                    {
                        case -8:
                            return CreateInstance<EveryTimespan>();
                        case -7:
                            return CreateInstance<TimerEquals>();
                    }
                    break;

                case -6:
                    switch (num)
                    {
                        case -1:
                            return CreateInstance<UponPressing>();

                        case -2:
                            return CreateInstance<RepeatWhilePressed>();
                    }
                    break;
                case 7:
                    switch (num)
                    {
                        case -81:
                            return CreateInstance<GetCounterValue>();
                    }
                    break;

            }
            Debug.LogError($"Missing condition: [Type: {objectType}; Code: {num}].");
            return null;
        }
    }
}
