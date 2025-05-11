using Ctf2Unity;
using Ctf2Unity.Runtime;
using Ctf2Unity.Runtime.ObjectInfoTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class ObjectInfoHolder : MonoBehaviour
{
    public GameObject prefab;
    public int handle;
    new public string name;
    public Constants.ObjectType objectType;
    public int reserved;
    public int inkEffect;
    public byte blendCoeff;
    public Color rgbCoeff;
    public bool transparent;

    public virtual Type InstanceType => typeof(ObjectInstance);

    public GameObject CreateInstance(Vector2Int pixelPosition, int layer, int order)
    {
        bool hasPrefab = prefab != null;

        var go = hasPrefab ?
#if UNITY_EDITOR
            (Application.isPlaying ? Instantiate(prefab) : (GameObject)PrefabUtility.InstantiatePrefab(prefab))
#else
            Instantiate(prefab)
#endif
            : new GameObject(handle.ToString());

        ObjectInstance comp = hasPrefab ? go.GetComponent<ObjectInstance>() : (ObjectInstance)go.AddComponent(InstanceType);
        comp.info = this;
        if (!hasPrefab)
        {
#if UNITY_EDITOR
            comp.Create();
#endif
        }

        comp.PixelPosition = pixelPosition;

        comp.UpdateLayer(layer, false);

        if (Application.isPlaying)
        {
            comp.UpdateOrder(order);
        }
        else
        {
            comp.SetOwnOrder(order);
            comp.UpdateLists();
        }

        if (!hasPrefab)
        {
#if UNITY_EDITOR
            prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(go, @"Assets\Objects\" + handle.ToString() + ".prefab", InteractionMode.AutomatedAction);
#endif
        }
        return go;
    }

#if UNITY_EDITOR
    protected virtual void Create(ObjectInfoReader info, Ctf2UnityProcessor processor)
    {

    }

    public static ObjectInfoHolder Create(ObjectInfoReader info, Transform parent, Ctf2UnityProcessor processor)
    {
        var go = new GameObject(info.Name);
        go.transform.parent = parent;

        Type compType;
        // add the right info holder type
        switch (info.ObjectType)
        {
            case Constants.ObjectType.Active:
                compType = typeof(ActiveObjectInfo);
                break;
                
            case Constants.ObjectType.Backdrop:
                compType = typeof(BackdropInfo);
                break;
            case Constants.ObjectType.QuickBackdrop:
                compType = typeof(QuickBackdropInfo);
                break;
            case Constants.ObjectType.Counter:
                compType = typeof(CounterInfo);
                break;
            default:
                compType = info.Properties.Loader is ObjectCommon ? typeof(CommonObjectInfoBase) : typeof(ObjectInfoHolder);
                break;
        }

        var comp = (ObjectInfoHolder)go.AddComponent(compType);
        comp.UpdateValues(info);
        comp.Create(info, processor);
        return comp;
    }

    private void UpdateValues(ObjectInfoReader reader)
    {
        handle = reader.Handle;
        name = reader.Name;
        objectType = reader.ObjectType;
        reserved = reader.Reserved;
        inkEffect = reader.InkEffect;
        blendCoeff = reader.BlendCoeff;
        rgbCoeff = reader.RGBCoeff;
        transparent = reader.Transparent;
    }
#endif
}

