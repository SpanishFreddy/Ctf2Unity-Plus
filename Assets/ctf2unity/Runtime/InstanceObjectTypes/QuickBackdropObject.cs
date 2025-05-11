using Ctf2Unity.Runtime.ObjectInfoTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Ctf2Unity.Runtime.InstanceObjectTypes
{
    public class QuickBackdropObject : BackdropBase
    {
        new public QuickBackdropInfo Info => (QuickBackdropInfo)info;

#if UNITY_EDITOR
        public override void Create()
        {
            base.Create();

            spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            spriteRenderer.size = Info.size;
            if (Info.obstacleType != Obstacle.Solid)
                return;
            boxCollider.SetTiledShape(spriteRenderer);
        }
#endif
    }
}
