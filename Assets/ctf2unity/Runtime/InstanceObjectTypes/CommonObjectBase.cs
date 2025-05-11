using Ctf2Unity.Runtime.ObjectInfoTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Ctf2Unity.Runtime.InstanceObjectTypes
{
    public class CommonObjectBase : ObjectInstance
    {
        public CommonObjectInfoBase Info => (CommonObjectInfoBase)info;
        public UnityEvent visibilityChanged = new UnityEvent();

        private VisibilityState visibility;
        public VisibilityState Visibility
        {
            get => visibility;
            set
            {
                if (value == visibility)
                    return;

                visibility = value;
                visibilityChanged.Invoke();
            }
        }

        protected override void OnAwake()
        {
            if (!Info.createAtStart)
            {
                Destroy(gameObject);
                return;
            }

            base.OnAwake();
        }

        public override void UpdateLists()
        {
            Info.SortInstance(this);

            base.UpdateLists();
        }

        protected override void OnDestroyed()
        {
            Info.RemoveInstance(this);

            base.OnDestroyed();
        }

        protected override void SetLayerParent(int layer)
        {
            if (Info.followFrame)
                base.SetLayerParent(layer);
            else
                transform.parent = FrameHandler.current.layers[layer].layerCam.transform;
        }
    }
}
