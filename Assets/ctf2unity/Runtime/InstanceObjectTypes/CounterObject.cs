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
    public class CounterObject : CommonObjectBase
    {
        public UnityEvent valueChanged = new UnityEvent();

        new public CounterInfo Info => (CounterInfo)info;

        [SerializeField] [HideInInspector] private double _value;
        public double Value
        {
            get => _value;
            set
            {
                if (_value == value)
                    return;

                _value = value.Clamp(Info.minValue, Info.maxValue);
                UpdateSprites();

                valueChanged.Invoke();
            }
        }

        public bool SpritesEnabled
        {
            get => sprites.Count == 0 || sprites[0].enabled;
            set
            {
                if (value == sprites[0].enabled)
                    return;

                for (int a = 0; a < sprites.Count; a++)
                {
                    var spr = sprites[a];
                    spr.enabled = value;
                }
            }
        }

        public List<SpriteRenderer> sprites = new List<SpriteRenderer>();

        protected override void OnAwake()
        {
            base.OnAwake();
            if (!Info.createAtStart)
                return;

            onLayerChange.AddListener(LayerChanged);
            onOrderChange.AddListener(OrderChanged);
            UpdateSprites();
        }

        public void OrderChanged()
        {
            for (int a = 0; a < sprites.Count; a++)
            {
                var spr = sprites[a];
                spr.sortingOrder = Order;
            }
        }

        public void LayerChanged()
        {
            var l = Layer;
            for (int a = 0; a < sprites.Count; a++)
            {
                var spr = sprites[a];
                spr.gameObject.layer = l;
            }
        }

        private void UpdateSprites()
        {
            var str = _value.ToString();
            var strLen = str.Length;

            if (strLen > sprites.Count)
            {
                var difference = strLen - sprites.Count;
                for (int a = 0; a < difference; a++)
                {
                    var idx = sprites.Count;
                    var go = new GameObject(idx.ToString());
                    go.layer = Layer;

                    var t = go.transform;
                    t.parent = transform;
                    t.localPosition = new Vector2(-Info.digitSize.x * (sprites.Count + 1), Info.digitSize.y);

                    var comp = go.AddComponent<SpriteRenderer>();
                    comp.enabled = SpritesEnabled;
                    comp.sortingOrder = Order;
                    sprites.Add(comp);
                }
            }
            else if (strLen < sprites.Count)
            {
                var difference = sprites.Count - strLen;
                for (int a = sprites.Count - difference; a < sprites.Count; a++)
                {
                    var spr = sprites[a];
                    Destroy(spr.gameObject);
                }
                sprites.RemoveRange(sprites.Count - difference, difference);
            }

            for (int a = 0; a < sprites.Count; a++)
            {
                var b = sprites.Count - a - 1;
                var c = str[a];
                sprites[b].sprite = GetSprite(c);
            }
        }

        public void VisibilityChange()
        {
            switch (Visibility)
            {
                case VisibilityState.Visible:
                    SpritesEnabled = true;
                    break;
                case VisibilityState.Invisible:
                    SpritesEnabled = false;
                    break;
                case VisibilityState.Flash:
                    // Implement
                    break;
            }
        }

        private Sprite GetSprite(char c)
        {
            int val = c - '0';
            if (val >= 0 && val <= 9)
                return Info.sprites[val];

            switch (c)
            {
                case '-':
                    return Info.sprites[10];

                case '+':
                    return Info.sprites[11];

                case '.':
                    return Info.sprites[12];

                case 'E':
                    return Info.sprites[13];
            }
            return null;
        }

        protected override void InitializeEvents()
        {
            base.InitializeEvents();

            onOrderChange.AddListener(OrderChanged);
            onLayerChange.AddListener(LayerChanged);
            visibilityChanged.AddListener(VisibilityChange);
        }

#if UNITY_EDITOR
        public override void Create()
        {
            base.Create();

            Value = Info.initialValue;
            if (Value == 0)
                UpdateSprites();
        }
#endif
    }
}
