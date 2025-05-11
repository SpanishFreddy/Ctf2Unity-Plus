using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.ObjectInfoTypes
{
    public abstract class BackdropInfoBase : ObjectInfoHolder
    {
        public Collision collisionType;
        public Obstacle obstacleType;
        public Sprite image;
    }
}
