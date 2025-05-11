using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ctf2Unity.Runtime.ObjectInfoTypes.Active
{
    [Serializable]
    public class AnimationDirectionInfo
    {
        public int speed;
        public int repeat;
        public int backTo;
        public bool loop;
        public AnimationFrame[] frames;

#if UNITY_EDITOR
        public AnimationDirectionInfo(AnimationDirection dir, bool generateColliderPoints, Ctf2UnityProcessor processor)
        {
            if (dir == null)
                return;

            speed = dir.MaxSpeed;
            repeat = dir.Repeat;
            backTo = dir.BackTo;
            loop = dir.Repeat < 1;

            frames = new AnimationFrame[dir.frames.Count];
            for (int a = 0; a < dir.frames.Count; a++)
            {
                var sprite = processor.Images.GetImageSprite(dir.frames[a]);
                frames[a] = new AnimationFrame(sprite);

                if (!generateColliderPoints)
                    continue;

                if (sprite == null)
                {
                    frames[a].colliderPoints = Array.Empty<ColliderShape>();
                    continue;
                }

                frames[a].colliderPoints = sprite.GetPhysicsPoints();
            }
        }
#endif
    }
}