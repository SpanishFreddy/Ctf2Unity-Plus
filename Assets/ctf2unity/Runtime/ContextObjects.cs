using Ctf2Unity.Runtime.InstanceObjectTypes;
using Ctf2Unity.Runtime.ObjectInfoTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ctf2Unity.Runtime
{
    public class ContextObjects : List<CommonObjectBase>
    {
        public CommonObjectInfoBase objectInfo;

        public ContextObjects(CommonObjectInfoBase objectInfo)
        {
            this.objectInfo = objectInfo;
        }

        public void SafeAddObject(CommonObjectBase obj)
        {
            var count = Count;
            for (int a = 0; a < count; a++)
            {
                var exObj = this[a];
                if (exObj == obj)
                    return;
            }

            for (int a = 0; a < count; a++)
            {
                var ins = this[a];
                if (ins.Layer < obj.Layer || (ins.Layer == ins.Layer && ins.Order < obj.Order))
                {
                    Insert(a, obj);
                    return;
                }
            }

            Add(obj);
        }
    }
}
