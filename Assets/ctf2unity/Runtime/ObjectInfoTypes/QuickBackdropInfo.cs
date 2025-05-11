using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.ObjectInfoTypes
{
    public class QuickBackdropInfo : BackdropInfoBase
    {
        public Vector2 size;

        public override Type InstanceType => typeof(QuickBackdropObject);

#if UNITY_EDITOR
        protected override void Create(ObjectInfoReader info, Ctf2UnityProcessor processor)
        {
            base.Create(info, processor);

            if (info.Properties.Loader is Quickbackdrop bd)
            {
                obstacleType = bd.ObstacleType;
                collisionType = bd.CollisionType;
                image = processor.Images.GetImageSprite(bd.Shape.Image);
                size = new Vector2(bd.Width / 100f, bd.Height / 100f);
            }
        }
#endif
    }
}
