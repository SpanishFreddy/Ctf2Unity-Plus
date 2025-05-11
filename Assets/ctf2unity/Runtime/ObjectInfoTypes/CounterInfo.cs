using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.ObjectInfoTypes
{
    public class CounterInfo : CommonObjectInfoBase
    {
        public override Type InstanceType => typeof(CounterObject);

        public Sprite[] sprites;

        public Vector2 digitSize;

        public int initialValue;
        public int maxValue;
        public int minValue;

#if UNITY_EDITOR
        protected override void Create(ObjectInfoReader info, Ctf2UnityProcessor processor)
        {
            base.Create(info, processor);

            if (info.Properties.Loader is ObjectCommon common)
            {
                var frames = common.Counters.frames;
                sprites = new Sprite[frames.Count];
                for (int i = 0; i < frames.Count; i++)
                {
                    sprites[i]=processor.Images.GetImageSprite(frames[i]);
                }
                initialValue = common.Counter.Initial;
                minValue = common.Counter.Minimum;
                maxValue = common.Counter.Maximum;

                var first = sprites[0].bounds.size;
                digitSize = first;
            }
        }
#endif
    }

}

