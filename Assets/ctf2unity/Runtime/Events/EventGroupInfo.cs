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
    public class EventGroupInfo
    {
        [NonSerialized] public EventContext context = new EventContext();

        public List<ConditionsGroup> conditionGroups = new List<ConditionsGroup>();
        public List<EventActionBase> actions = new List<EventActionBase>();

        public void Execute()
        {
            var count = actions.Count;
            for (int a = 0; a < count; a++)
            {
                var act = actions[a];
                act.Execute();
            }
        }

        public void Initialize()
        {
            for (int a = 0; a < conditionGroups.Count; a++)
            {
                var c = conditionGroups[a];
                c.context = context;
                c.Initialize();
                c.onConditionsMet.AddListener(Execute);
            }
            for (int a = 0; a < actions.Count; a++)
            {
                var c = actions[a];
                c.context = context;
                c.Initialize();
            }
        }

        public void Update()
        {
            var count = conditionGroups.Count;
            for (int a = 0; a < count; a++)
            {
                var c = conditionGroups[a];
                c.Update();
            }
        }

        public void LateUpdate()
        {
            var count = conditionGroups.Count;
            for (int a = 0; a < count; a++)
            {
                var c = conditionGroups[a];
                c.LateUpdate();
            }
        }
    }
}
