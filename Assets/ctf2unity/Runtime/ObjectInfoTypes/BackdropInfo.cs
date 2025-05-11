using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.ObjectInfoTypes
{
    public class BackdropInfo : BackdropInfoBase
    {
        public override Type InstanceType => typeof(BackdropObject);

#if UNITY_EDITOR
        protected override void Create(ObjectInfoReader info, Ctf2UnityProcessor processor)
        {
            base.Create(info, processor);

            if (info.Properties.Loader is Backdrop bd)
            {
                obstacleType = bd.ObstacleType;
                collisionType = bd.CollisionType;
                image = processor.Images.GetImageSprite(bd.Image);
            }
        }
#endif
    }
}