using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Ctf2Unity.Runtime
{
    [Serializable]
    public struct AssetReferences
    {
        public Material spriteAddition;

#if UNITY_EDITOR
        public void Update()
        {
            spriteAddition = AssetDatabase.LoadAssetAtPath<Material>(@"Assets\ctf2unity\Runtime\Shaders\Sprite Addition.mat");
        }
#endif
    }
}
