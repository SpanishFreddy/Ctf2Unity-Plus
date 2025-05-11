using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ctf2Unity.Runtime
{
    public class ValueChangeEventArgs<T> : EventArgs
    {
        public readonly T value;

        public ValueChangeEventArgs(T value)
        {
            this.value = value;
        }

        public static implicit operator ValueChangeEventArgs<T>(T value)
        {
            return new ValueChangeEventArgs<T>(value);
        }
    }
}
