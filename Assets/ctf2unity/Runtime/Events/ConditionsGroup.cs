using Ctf2Unity.Runtime.Events.Conditions.Handlers;
using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Ctf2Unity.Runtime.Events
{
    [Serializable]
    public class ConditionsGroup
    {
        [NonSerialized] public EventContext context;
        [NonSerialized] public UnityEvent onConditionsMet = new UnityEvent();

        private int eventsTrueCount = 0;

        public List<ConditionsHandlerBase> handlers = new List<ConditionsHandlerBase>();
        public List<EventConditionBase> eventConditions = new List<EventConditionBase>();
        public List<CheckConditionBase> checkConditions = new List<CheckConditionBase>();

        public void Initialize()
        {
            for (int a = 0; a < eventConditions.Count; a++)
            {
                var e = eventConditions[a];
                e.context = context;
                e.Initialize();
                e.onEvent.AddListener(OnEvent);
            }
            for (int a = 0; a < checkConditions.Count; a++)
            {
                var e = checkConditions[a];
                e.context = context;
            }
            for (int a = 0; a < handlers.Count; a++)
            {
                var e = handlers[a];
                e.conditionsGroup = this;
            }
        }

        public void CheckConditionsMet()
        {
            bool check = true;
            var count = handlers.Count;
            for (int a = 0; a < count; a++)
            {
                var c = handlers[a];
                if (!c.HandleBeforeCheck())
                    check = false;
            }

            bool shouldInvoke = true;
            if (check)
            {
                var count2 = checkConditions.Count;
                for (int a = 0; a < count2; a++)
                {
                    var c = checkConditions[a];
                    var flag = c.Check();
                    if (!flag)
                    {
                        shouldInvoke = false;
                        break;
                    }
                }
            }

            bool invoke = shouldInvoke;
            for (int a = 0; a < count; a++)
            {
                var c = handlers[a];
                if (!c.HandleResult(shouldInvoke))
                    invoke = false;
            }

            if (invoke)
                onConditionsMet.Invoke();
        }

        public void OnEvent()
        {
            eventsTrueCount++;
            if (eventsTrueCount == eventConditions.Count)
            {
                CheckConditionsMet();
            }
        }

        public void Update()
        {
            if (eventConditions.Count == 0)
            {
                CheckConditionsMet();
            }
        }

        public void LateUpdate()
        {
            eventsTrueCount = 0;

            bool clear = true;
            var count = handlers.Count;
            for (int a = 0; a < count; a++)
            {
                var c = handlers[a];
                if (!c.HandleClearContextObjects())
                    clear = false;
            }
            if (clear)
                context.Reset();
        }

        public void AddCondition(ConditionBase condition)
        {
            if (condition is CheckConditionBase check)
            {
                checkConditions.Add(check);
                return;
            }
            if (condition is EventConditionBase eventt)
            {
                eventConditions.Add(eventt);
                return;
            }
            if (condition is ConditionsHandlerBase handler)
            {
                handlers.Add(handler);
                return;
            }
        }
    }
}
