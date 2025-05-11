using Ctf2Unity.Runtime.InstanceObjectTypes;
using Ctf2Unity.Runtime.ObjectInfoTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ctf2Unity.Runtime.Events
{
    public class EventContext
    {
        public List<ContextObjects> objects = new List<ContextObjects>();

        public void SafeAddObject(CommonObjectBase obj)
        {
            GetContextObjectsList(obj.Info).SafeAddObject(obj);
        }

        public ContextObjects GetObjects(CommonObjectInfoBase objectInfo)
        {
            var count = objects.Count;
            for (int a = 0; a < count; a++)
            {
                var obj = objects[a];
                if (obj.objectInfo == objectInfo)
                    return obj;
            }
            return null;
        }

        private ContextObjects GetContextObjectsList(CommonObjectInfoBase objectInfo)
        {
            var objs = GetObjects(objectInfo);
            if (objs != null)
                return objs;

            var con = new ContextObjects(objectInfo);
            objects.Add(con);
            return con;
        }

        public void SafeAddObjects(CommonObjectBase[] objs)
        {
            var count = objs.Length;
            for (int a = 0; a < count; a++)
            {
                var obj = objs[a];
                SafeAddObject(obj);
            }
        }

        public void Reset()
        {
            objects.Clear();
        }
    }
}
