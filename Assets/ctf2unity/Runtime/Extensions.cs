using Ctf2Unity.Runtime.InstanceObjectTypes;
using Ctf2Unity.Runtime.ObjectInfoTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime
{
    public static class Extensions
    {
        public static void ApplyEffect(this SpriteRenderer spriteRenderer, int index)
        {
            switch (index)
            {
                case 9:
                    spriteRenderer.sharedMaterial = AppInfo.current.assets.spriteAddition;
                    break;
            }
        }

        public static void SetPhysicsPoints(this PolygonCollider2D collider, ColliderShape[] points)
        {
            var len = points.Length;
            collider.pathCount = len;
            for (int a = 0; a < len; a++)
            {
                collider.SetPath(a, points[a].points);
            }
        }

        public static ColliderShape[] GetPhysicsPoints(this Sprite sprite)
        {
            var count = sprite.GetPhysicsShapeCount();
            var result = new ColliderShape[count];
            for (int i = 0; i < count; i++)
            {
                List<Vector2> points = new List<Vector2>();
                sprite.GetPhysicsShape(i, points);
                result[i].points = points;
            }
            return result;
        }

        public static void SetShape(this BoxCollider2D collider, Sprite sprite)
        {
            var spriteBounds = sprite.bounds;
            collider.size = spriteBounds.size;
            collider.offset = spriteBounds.center;
        }

        public static void SetTiledShape(this BoxCollider2D collider, SpriteRenderer sprite)
        {
            collider.size = sprite.size;
            collider.offset = sprite.bounds.center;
        }

        public static int GetNearest(this IEnumerable<int> array, int targetNumber)
        {
            return array.Aggregate((current, next) => Math.Abs((long)current - targetNumber) < Math.Abs((long)next - targetNumber) ? current : next);
        }

        public static void CallLast(MonoBehaviour monoBehaviour, System.Action action)
        {
            monoBehaviour.StartCoroutine(CoCallLast(action));
        }

        private static IEnumerator CoCallLast(System.Action action)
        {
            yield return new WaitForEndOfFrame();
            action.Invoke();
        }

        public static KeyCode? GetKeyFromCTFKeyId(int id)
        {
            return default;
        }

        public static double Clamp(this double value, double min, double max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;

            return value;
        }

        public static CommonObjectBase GetTopmost(this List<CommonObjectBase> objects, CommonObjectInfoBase info)
        {
            CommonObjectBase result = null;
            var count = objects.Count;
            for (int a = 0; a < count; a++)
            {
                var ins = objects[a];
                if (ins.Info == info)
                {
                    if (result == null || result.Order < ins.Order)
                        result = ins;
                }
            }
            return result;
        }

        public static IOrderedEnumerable<CommonObjectBase> SortObjects(this List<CommonObjectBase> objs)
        {
            return objs.OrderBy(x => x.Layer).ThenBy(x => x.Order);
        }
    }
}
