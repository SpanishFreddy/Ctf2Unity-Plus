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
    public abstract class CommonObjectActionBase : EventActionBase
    {
        public CommonObjectInfoBase objectInfo;

        public override void Execute()
        {
            var objs = context.GetObjects(objectInfo);
            if (objs == null)
            {
                var count2 = objectInfo.Instances.Count;
                for (int a = 0; a < count2; a++)
                {
                    var ins = objectInfo.Instances[a];
                    Execute(ins, a);
                }
                return;
            }

            var count = objs.Count;
            for (int a = 0; a < count; a++)
            {
                var ins = objs[a];

                Execute(ins, a);
            }
        }

        public abstract void Execute(CommonObjectBase obj, int index);
    }
}
