using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ctf2Unity.Runtime.Events.ParameterTypes
{
    public class KeyParam : EventParameterBase
    {
        public KeyCode key;

#if UNITY_EDITOR
        public override void Create(ParameterCommon par)
        {
            var keyPar = (KeyParameter)par;
            key = PainInAss(keyPar.Key);
        }

        public static KeyCode PainInAss(short code) //Im fr gonna leave this name
        {
            switch (code)
            {
                case 27: //Escape
                case 32:
                    return (KeyCode)code;
            }

            if (code >= 65 && code <= 90) // Letters
                return (KeyCode)(code + 32);

            if (code >= 112 && code <= 126) // F keys
                return (KeyCode)(code + 170);

            if (code >= 48 && code <= 57) // Numbers
                return (KeyCode)code;

            if (code >= 96 && code <= 105) // NumPad keys
                return (KeyCode)(code + 160);

            return KeyCode.None;
        }
#endif
    }
}
