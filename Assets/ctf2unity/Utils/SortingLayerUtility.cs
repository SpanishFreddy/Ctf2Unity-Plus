using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Utils
{
    public static class SortingLayerUtility
    {
        public static SortingLayer GetOrCreateLayer(int index)
        {
            var layers = SortingLayer.layers;
            if (index < layers.Length)
            {
                return layers[index];
            }
            return default;
        }
    }
}
