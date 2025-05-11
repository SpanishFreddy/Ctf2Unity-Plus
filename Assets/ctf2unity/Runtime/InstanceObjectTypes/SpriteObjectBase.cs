using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.InstanceObjectTypes
{
    public abstract class SpriteObjectBase : CommonObjectBase
    {
        public SpriteRenderer spriteRenderer;


        public void VisibilityChange()
        {
            switch (Visibility)
            {
                case VisibilityState.Visible:
                    spriteRenderer.enabled = true;
                    break;
                case VisibilityState.Invisible:
                    spriteRenderer.enabled = false;
                    break;
                case VisibilityState.Flash:
                    // Implement
                    break;
            }
        }

        public void OrderChanged()
        {
            spriteRenderer.sortingOrder = Order;
        }

        protected override void InitializeEvents()
        {
            base.InitializeEvents();

            onOrderChange.AddListener(OrderChanged);
            visibilityChanged.AddListener(VisibilityChange);
        }

#if UNITY_EDITOR
        public override void Create()
        {
            base.Create();

            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(Info.rgbCoeff.r, Info.rgbCoeff.g, Info.rgbCoeff.b, 1f - Info.blendCoeff / 255f);

            spriteRenderer.ApplyEffect(Info.inkEffect);
        }
#endif
    }
}
