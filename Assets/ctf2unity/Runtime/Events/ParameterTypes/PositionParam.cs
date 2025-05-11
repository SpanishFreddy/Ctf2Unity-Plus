using Ctf2Unity.Runtime.ObjectInfoTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events.ParameterTypes
{
    public class PositionParam : EventParameterBase
    {
        public CommonObjectInfoBase parent;
        public Vector2 rawPosition;

        public Vector2? GetPosition(int index)
        {
            if (parent == null)
                return rawPosition;

            if (parent.Instances.Count == 0)
                return null;

            var ins = parent.Instances[index % parent.Instances.Count];
            var pos = ins.transform.position;
            return new Vector2(pos.x + rawPosition.x, pos.y + rawPosition.y);
        }

#if UNITY_EDITOR
        public override void Create(ParameterCommon par)
        {
            var pos = (Position)par;
            parent = (CommonObjectInfoBase)FrameHandler.current.GetObjectInfoByHandle(pos.ObjectInfoParent);
            rawPosition = new Vector2(pos.X / 100f, pos.Y / -100f);
        }
#endif
    }
}
