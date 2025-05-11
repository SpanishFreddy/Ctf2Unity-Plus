using Ctf2Unity.Runtime.InstanceObjectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.ObjectInfoTypes
{
    public class CommonObjectInfoBase : ObjectInfoHolder
    {
        public bool followFrame;
        public Color32 backColor;
        public bool visibleAtStart;
        public bool createAtStart;

        public List<int> altValues;
        public List<string> altStrings;

        [SerializeField] private List<CommonObjectBase> instances = new List<CommonObjectBase>();
        public IReadOnlyList<CommonObjectBase> Instances => instances;

        public void SortInstance(CommonObjectBase obj)
        {
            instances.Remove(obj);

            var count = instances.Count;

            for (int a = 0; a < count; a++)
            {
                var ins = instances[a];
                if (ins.Layer < obj.Layer || (ins.Layer == ins.Layer && ins.Order < obj.Order))
                {
                    instances.Insert(a, obj);
                    return;
                }
            }

            instances.Add(obj);
        }

        public void RemoveInstance(CommonObjectBase ins)
        {
            instances.Remove(ins);
        }

#if UNITY_EDITOR
        protected override void Create(ObjectInfoReader info, Ctf2UnityProcessor processor)
        {
            base.Create(info, processor);

            var common = (ObjectCommon)info.Properties.Loader;

            backColor = common.BackColor;
            followFrame = !common.Flags.HasFlag(ObjectCommon.ObjectCommonFlags.ScrollingIndependant);
            createAtStart = common.Flags.HasFlag(ObjectCommon.ObjectCommonFlags.DoNotCreateAtStart);
            visibleAtStart = common.NewFlags.HasFlag(ObjectCommon.ObjectCommonNewFlags.VisibleAtStart);

            altValues = common.Values == null ? new List<int>() : common.Values.Items;
            altStrings = common.Strings == null ? new List<string>() : common.Strings.Items;
        }
#endif
    }
}
