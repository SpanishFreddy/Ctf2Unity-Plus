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
    public abstract class BackdropBase : ObjectInstance
    {
        public BackdropInfoBase Info => (BackdropInfoBase)info;

        public SpriteRenderer spriteRenderer;

        public PolygonCollider2D polygonCollider;
        public BoxCollider2D boxCollider;

        public void OrderChanged()
        {
            spriteRenderer.sortingOrder = Order;
        }

        protected override void InitializeEvents()
        {
            base.InitializeEvents();

            onOrderChange.AddListener(OrderChanged);
        }

#if UNITY_EDITOR

        public override void Create()
        {
            base.Create();

            GameObjectUtility.SetStaticEditorFlags(gameObject, (StaticEditorFlags)~0);
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Info.image;

            if (Info.obstacleType != Obstacle.Solid)
                return;

            if (Info.collisionType == Collision.Box)
            {
                boxCollider = gameObject.AddComponent<BoxCollider2D>();
                boxCollider.SetShape(spriteRenderer.sprite);
                boxCollider.isTrigger = true;
            }
            else
            {
                polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
                polygonCollider.SetPhysicsPoints(spriteRenderer.sprite.GetPhysicsPoints());
                polygonCollider.isTrigger = true;
            }
        }
#endif
    }
}
