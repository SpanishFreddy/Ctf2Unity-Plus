using Ctf2Unity.Runtime.InstanceObjectTypes;
using Ctf2Unity.Runtime.ObjectInfoTypes.Active;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.ObjectInfoTypes
{
    public class ActiveObjectInfo : CommonObjectInfoBase
    {
        public SerializableDictionary<int, ObjectAnimationInfo> animations = new SerializableDictionary<int, ObjectAnimationInfo>();
        public bool collisionBox;

        public override Type InstanceType => typeof(ActiveObject);

#if UNITY_EDITOR
        protected override void Create(ObjectInfoReader info, Ctf2UnityProcessor processor)
        {
            base.Create(info, processor);

            if (info.Properties.Loader is ObjectCommon common)
            {
                collisionBox = common.NewFlags.HasFlag(ObjectCommon.ObjectCommonNewFlags.CollisionBox);
                int idx = 0;
                foreach (var a in common.Animations.AnimationDict)
                {
                    animations.Add(a.Key, new ObjectAnimationInfo(a.Value, !collisionBox, processor));
                    idx++;
                }


            }
        }
#endif
    }
}
