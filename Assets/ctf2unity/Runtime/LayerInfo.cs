using System;
using UnityEditor;
using UnityEngine;

namespace Ctf2Unity.Runtime
{
    public class LayerInfo : MonoBehaviour
    {
        public Camera layerCam;
        public int index;
        public Vector2 coefficient;

        public void SetCamPosition(Vector2 position)
        {
            transform.position = position * coefficient;
        }

#if UNITY_EDITOR
        public static LayerInfo Create(Layer layer, int index)
        {
            var go = new GameObject(layer.Name);
            var comp = go.AddComponent<LayerInfo>();

            comp.index = index;
            comp.coefficient = new Vector2(layer.XCoeff, layer.YCoeff);

            // Generate camera
            comp.layerCam = new GameObject("Layer Camera").AddComponent<Camera>();
            comp.layerCam.cullingMask = 1 << index;
            comp.layerCam.depth = index;
            comp.layerCam.orthographic = true;
            comp.layerCam.clearFlags = index == 0 ? CameraClearFlags.SolidColor : CameraClearFlags.Nothing;
            var height = AppInfo.current.resolution.y / 200f;
            comp.layerCam.orthographicSize = height;
            comp.layerCam.transform.parent = comp.transform;
            comp.layerCam.transform.position = new Vector3(AppInfo.current.resolution.x / 200f, -height, -10f);
            if (index == 0)
                comp.layerCam.backgroundColor = FrameHandler.current.backgroundColor;

            return comp;
        }
#endif
    }
}