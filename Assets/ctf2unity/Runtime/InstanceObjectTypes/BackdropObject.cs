using Ctf2Unity.Runtime.ObjectInfoTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Ctf2Unity.Runtime.InstanceObjectTypes
{
    public class BackdropObject : BackdropBase
    {
        new public BackdropInfo Info => (BackdropInfo)info;

#if UNITY_EDITOR
        public override void Create()
        {
            base.Create();
        }
#endif
    }
}
