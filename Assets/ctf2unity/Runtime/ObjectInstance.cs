using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Ctf2Unity.Runtime
{
    public class ObjectInstance : MonoBehaviour
    {
        public UnityEvent onLayerChange = new UnityEvent();
        public UnityEvent onOrderChange = new UnityEvent();

        //public int handle;
        [SerializeField] public ObjectInfoHolder info;

        [SerializeField] private int order;

        public Vector2 PixelPosition
        {
            get
            {
                var pos = transform.localPosition;
                return new Vector2(pos.x * 100f, pos.y * -100f);
            }
            set
            {
                transform.localPosition = new Vector2(value.x / 100f, value.y / -100f);
            }
        }

        public int Order
        {
            get => order;
            set
            {
                if (value == order)
                    return;

                UpdateOrder(value);
            }
        }

        public int Layer
        {
            get => gameObject.layer;
            set
            {
                if (value == Layer)
                    return;

                UpdateLayer(value, true);
            }
        }

        public ObjectInstance()
        {
            InitializeEvents();
        }

        public void UpdateOrder(int value)
        {
            var comps = FindObjectsOfType<ObjectInstance>(true);
            var len = comps.Length;
            var layer = Layer;
            int objs = 0;
            for (int a = 0; a < len; a++)
            {
                var comp = comps[a];
                if (comp.Layer == layer)
                {
                    objs++;
                    if (comp.Order >= value)
                        comp.SetOwnOrder(comp.Order + 1);
                }
            }

            SetOwnOrder(Mathf.Clamp(value, 0, objs));
            UpdateLists();
        }

        public void UpdateLayer(int value, bool updateOrder)
        {
            gameObject.layer = value;
            var t = transform;
            var pos = t.localPosition;
            SetLayerParent(Layer);
            t.localPosition = pos;

            if (updateOrder)
                Order = int.MaxValue; // This will get clamped

            onLayerChange.Invoke();
        }

        protected virtual void SetLayerParent(int layer)
        {
            transform.parent = FrameHandler.current.layers[layer].transform;
        }

        public void SetOwnOrder(int value)
        {
            order = value;
            onOrderChange.Invoke();
        }

        private void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        private void OnDestroy()
        {
            OnDestroyed();
        }

        public virtual void UpdateLists() { }
        protected virtual void OnDestroyed() { }
        protected virtual void InitializeEvents() { }
        protected virtual void OnAwake() { }
        protected virtual void OnStart() { }

#if UNITY_EDITOR
        public virtual void Create()
        {
        }
#endif
    }
}
