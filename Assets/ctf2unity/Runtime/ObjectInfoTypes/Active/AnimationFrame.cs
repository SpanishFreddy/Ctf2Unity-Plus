using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.ObjectInfoTypes.Active
{
    [Serializable]
    public class AnimationFrame
    {
        public Sprite sprite;
        [SerializeField] public ColliderShape[] colliderPoints;

        public AnimationFrame(Sprite sprite, ColliderShape[] colliderPoints = null)
        {
            this.sprite = sprite;
            this.colliderPoints = colliderPoints;
        }
    }
}
