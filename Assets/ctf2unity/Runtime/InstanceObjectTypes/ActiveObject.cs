using Ctf2Unity.Runtime.ObjectInfoTypes;
using Ctf2Unity.Runtime.ObjectInfoTypes.Active;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Ctf2Unity.Runtime.InstanceObjectTypes
{
    public class ActiveObject : SpriteObjectBase
    {
        public bool isAnimating;
        private int repeated = 0;
        private bool alreadyPlayed;

        public int overrideFrame = -1;
        public int overrideSpeed = 0;

        public UnityEvent onAnimationFinished = new UnityEvent();

        private int currentDirection = 0;
        private int currentAnimationIndex = -1;
        private int currentAnimationDirectionIndex = -1;
        [SerializeField] [HideInInspector] private ObjectAnimationInfo currentAnimation;
        [SerializeField] [HideInInspector] private AnimationDirectionInfo currentAnimationDirection;
        public ObjectAnimationInfo CurrentAnimation => currentAnimation;
        public AnimationDirectionInfo CurrentAnimationDirection => currentAnimationDirection;
        private int currentFrame = -1;

        public int CurrentDirection
        {
            get => currentDirection;
            set
            {
                if (value == currentDirection || value > 31 || value < 0)
                    return;

                currentDirection = value;
                UpdateDirection();
            }
        }

        public int CurrentAnimationIndex
        {
            get => currentAnimationIndex;
            set
            {
                if (value == currentAnimationIndex)
                    return;

                if (value > Info.animations.Count - 1 || value < 0)
                {
                    currentAnimationIndex = -1;
                    currentAnimation = null;
                    CurrentAnimationDirectionIndex = -1;
                    return;
                }

                if (Info.animations.TryGetValue(value, out ObjectAnimationInfo anim))
                {
                    currentAnimationIndex = value;
                    currentAnimation = anim;

                    UpdateDirection();
                    return;
                }

                currentAnimationIndex = -1;
                currentAnimation = null;
                CurrentAnimationDirectionIndex = -1;
            }
        }

        public int CurrentAnimationDirectionIndex
        {
            get => currentAnimationDirectionIndex;
            private set
            {
                if (CurrentAnimation != null && value >= 0)
                {
                    if (value == 32)
                        value = 0;

                    if (CurrentAnimation.directions.TryGetValue(value, out AnimationDirectionInfo dir))
                    {
                        if (CurrentAnimationDirection == dir)
                            return;
                        currentAnimationDirectionIndex = value;
                        currentAnimationDirection = dir;
                        CurrentFrameIndex = 0;
                        repeated = 0;
                        alreadyPlayed = false;
                        if (Application.isPlaying)
                            StartAnimating(true);
                        return;
                    }
                }

                currentAnimationDirectionIndex = -1;
                currentAnimationDirection = null;
                CurrentFrameIndex = -1;
                repeated = 0;
                alreadyPlayed = false;
                if (Application.isPlaying)
                    StopAnimating();
                return;
            }
        }

        public Sprite CurrentFrame => spriteRenderer.sprite;
        public int CurrentFrameIndex
        {
            get => currentFrame;
            set
            {
                if (CurrentAnimationDirection == null || CurrentAnimationDirection.frames == null || CurrentAnimationDirection.frames.Length == 0)
                {
                    currentFrame = -1;
                    spriteRenderer.sprite = null;
                    return;
                }

                currentFrame = Mathf.Clamp(value, 0, CurrentAnimationDirection.frames.Length - 1);
                var sprite = CurrentAnimationDirection.frames[currentFrame].sprite;
                if (spriteRenderer.sprite == sprite)
                    return;

                spriteRenderer.sprite = sprite;
                UpdateCollider();
            }
        }

        public IEnumerator Animate()
        {
            if (CurrentAnimationDirection != null)
            {
                if (CurrentAnimationDirection.loop && CurrentAnimationDirection.frames.Length == 1)
                    yield break; // This will stop the coroutine if there isnt really anything to animate

                var frames = CurrentAnimationDirection.frames.Length;
                if (frames != 0)
                    for (; ; )
                    {
                        if (overrideFrame < 0)
                        {
                            if (CurrentFrameIndex + 1 >= frames)
                            {
                                if (!CurrentAnimationDirection.loop)
                                {
                                    repeated++;
                                    if (repeated >= CurrentAnimationDirection.repeat)
                                        break;
                                }
                                else if (CurrentFrameIndex == CurrentAnimationDirection.backTo)
                                    yield break; // This will stop the coroutine if there isnt really anything to animate anymore

                                CurrentFrameIndex = CurrentAnimationDirection.backTo;
                            }
                            else
                            {
                                CurrentFrameIndex++;
                            }
                        }
                        else
                        {
                            if (CurrentFrameIndex != overrideFrame)
                                CurrentFrameIndex = overrideFrame;
                        }
                        yield return new WaitForSeconds(1f / (AppInfo.current.maxFps * ((overrideSpeed > 0 ? Mathf.Clamp(overrideSpeed, 1, 100) : CurrentAnimationDirection.speed) / 100f)));
                    }
            }

            onAnimationFinished.Invoke();
            isAnimating = false;
            alreadyPlayed = true;
        }

        public void StartAnimating(bool reset = false)
        {
            if (reset)
            {
                repeated = 0;
                alreadyPlayed = false;
            }
            else if (alreadyPlayed)
                return;
            isAnimating = true;

            StartCoroutine(Animate());
        }

        public void StopAnimating()
        {
            StopCoroutine(Animate());
            isAnimating = false;
        }

        private void UpdateDirection()
        {
            if (CurrentAnimation == null)
                return;

            var count = CurrentAnimation.directions.Count;
            if (count == 0)
            {
                CurrentAnimationDirectionIndex = -1;
                return;
            }
            if (count == 1)
            {
                CurrentAnimationDirectionIndex = CurrentAnimation.directions.First().Key;
                return;
            }

            bool needs32 = CurrentAnimation.directions.Any(x => x.Key == 0);
            var keys = new int[needs32 ? count + 1 : count];
            int idx = 0;
            foreach (var a in CurrentAnimation.directions)
            {
                keys[idx] = a.Key;
                idx++;
            }
            if (needs32)
                keys[count] = 32;

            CurrentAnimationDirectionIndex = keys.GetNearest(currentDirection);
        }

        public PolygonCollider2D polygonCollider;
        public BoxCollider2D boxCollider;

        new public ActiveObjectInfo Info => (ActiveObjectInfo)info;

        public void UpdateCollider()
        {
            if (Info.collisionBox)
            {
                if (CurrentFrame == null)
                {
                    boxCollider.enabled = false;
                    return;
                }
                boxCollider.enabled = true;
                boxCollider.SetShape(spriteRenderer.sprite);
            }
            else
            {
                if (CurrentFrame == null)
                {
                    polygonCollider.enabled = false;
                    return;
                }
                polygonCollider.enabled = true;
                polygonCollider.SetPhysicsPoints(CurrentAnimationDirection.frames[currentFrame].colliderPoints);
            }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            if (!Info.createAtStart)
                return;

            if (!Info.visibleAtStart)
                Visibility = VisibilityState.Invisible;
        }

        private void OnEnable()
        {
            if (!isAnimating)
                StartAnimating(true);
        }

#if UNITY_EDITOR
        public override void Create()
        {
            base.Create();

            if (Info.collisionBox)
            {
                boxCollider = gameObject.AddComponent<BoxCollider2D>();
                boxCollider.isTrigger = true;
            }
            else
            {
                polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
                polygonCollider.isTrigger = true;
            }

            CurrentAnimationIndex = 0;
        }
#endif
    }
}
