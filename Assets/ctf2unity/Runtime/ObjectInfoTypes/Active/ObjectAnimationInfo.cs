using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ctf2Unity.Runtime.ObjectInfoTypes.Active
{
    [Serializable]
    public class ObjectAnimationInfo
    {
        public SerializableDictionary<int, AnimationDirectionInfo> directions = new SerializableDictionary<int, AnimationDirectionInfo>();

#if UNITY_EDITOR
        public ObjectAnimationInfo(Animation anim, bool generateColliderPoints, Ctf2UnityProcessor processor)
        {
            if (anim == null || anim.DirectionDict == null)
                return;

            foreach (var a in anim.DirectionDict)
            {
                directions.Add(a.Key, new AnimationDirectionInfo(a.Value, generateColliderPoints, processor));
            }
        }
#endif

        public AnimationDirectionInfo GetDirection(int angle)
        {
            return default;
            // Idk how to get this
        }
    }
}
