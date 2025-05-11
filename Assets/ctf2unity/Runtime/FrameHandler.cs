using Ctf2Unity.Runtime.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Ctf2Unity.Runtime
{
    public class FrameHandler : MonoBehaviour
    {
        public static FrameHandler current;

        public UnityEvent onStart = new UnityEvent();
        public UnityEvent onEnd = new UnityEvent();

        [SerializeField] [HideInInspector] private AppInfo app;
        public Vector2 frameSize;
        public Vector2 virtualFrameSize;
        public Color backgroundColor;

        public LayerInfo[] layers;
        private Vector2 camPosition;
        public UInt64 frameTimer;

        public List<EventGroupInfo> events = new List<EventGroupInfo>();

        public List<ObjectInfoHolder> objectInfos = new List<ObjectInfoHolder>();

        public Vector2 CamPosition
        {
            get => camPosition;
            set
            {
                var offsetX = app.resolution.x / 200f;
                var offsetY = app.resolution.y / -200f;
                var position = new Vector2(Mathf.Clamp(value.x, offsetX, frameSize.x - offsetX), Mathf.Clamp(value.y, offsetY, frameSize.y - offsetY));
                if (position == camPosition)
                    return;

                camPosition = position;
                var len = layers.Length;
                for (int a = 0; a < len; a++)
                {
                    var l = layers[a];
                    l.SetCamPosition(position);
                }
            }
        }

        public Vector2Int CTFCamPosition
        {
            get => new Vector2Int((int)(camPosition.x * 100f), (int)(camPosition.y * -100f));
            set
            {
                CamPosition = new Vector2(value.x / 100f, value.y / -100f);
            }
        }

        public ObjectInfoHolder GetObjectInfoByHandle(int handle)
        {
            var count = objectInfos.Count;
            for (int a = 0; a < count; a++)
            {
                var info = objectInfos[a];
                if (info.handle == handle)
                    return info;
            }
            return null;
        }

        public FrameHandler()
        {
            current = this;
        }

        private void Awake()
        {
            Application.quitting += onEnd.Invoke;

            for (int a = 0; a < events.Count; a++)
            {
                var e = events[a];
                e.Initialize();
            }
        }

        private void Start()
        {
            onStart.Invoke();
        }

        private void Update()
        {
            var count = events.Count;
            for (int a = 0; a < count; a++)
            {
                var e = events[a];
                e.Update();
            }
            frameTimer += (ulong)(1.0f / UnityEngine.Time.deltaTime);
        }

        private void LateUpdate()
        {
            var count = events.Count;
            for (int a = 0; a < count; a++)
            {
                var e = events[a];
                e.LateUpdate();
            }
        }

        public void NextFrame()
        {
            JumpToFrame(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void PreviousFrame()
        {
            JumpToFrame(SceneManager.GetActiveScene().buildIndex - 1);
        }

        public void JumpToFrame(int index)
        {
            onEnd.Invoke();

            if (index >= SceneManager.sceneCountInBuildSettings)
            {
                Application.Quit();
                return;
            }
            if (index < 0)
                index = 0;

            Extensions.CallLast(this, () => SceneManager.LoadScene(index, LoadSceneMode.Single)); //Makes sure it doesnt switch the scene while actions are still executing
        }

        public void EndApp()
        {
            Debug.LogError("Is trying to quit");
            Extensions.CallLast(this, () => Application.Quit());
        }

#if UNITY_EDITOR
        public void Create(Frame frame)
        {
            var col = frame.Background;
            backgroundColor = new Color(col.r, col.g, col.b, col.a) / 255f;
            frameSize = new Vector2(frame.Width / 100f, frame.Height / -100f);
            virtualFrameSize = new Vector2(frame.VirtWidth / 100f, frame.VirtHeight / -100f);

            var ls = frame.Layers;
            layers = new LayerInfo[ls.Count];
            for (int a = 0; a < ls.Count; a++)
            {
                var l = ls[a];
                layers[a] = LayerInfo.Create(l, a);
            }
            app = AppInfo.current;
        }
#endif
    }
}
