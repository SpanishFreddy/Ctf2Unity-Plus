using Ctf2Unity.Runtime.InstanceObjectTypes;
using Ctf2Unity.Runtime.ObjectInfoTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events
{
    [Serializable]
    public abstract class CommonObjectConditionBase : CheckConditionBase
    {
        public CommonObjectInfoBase objectInfo;

        public override bool Check()
        {
            var objs = context.GetObjects(objectInfo);
            bool met = false;
            if (objs == null)
            {
                var count2 = objectInfo.Instances.Count;
                for (int a = 0; a < count2; a++)
                {
                    var ins = objectInfo.Instances[a];
                    if (Check(ins, a))
                    {
                        context.SafeAddObject(ins);
                        met = true;
                    }
                }
                return met;
            }

            var count = objs.Count;
            for (int a = 0; a < count; a++)
            {
                var ins = objs[a];

                if (Check(ins, a))
                {
                    objs.SafeAddObject(ins);
                    met = true;
                }
            }
            return met;
        }

        public abstract bool Check(CommonObjectBase obj, int index);
    }
}
