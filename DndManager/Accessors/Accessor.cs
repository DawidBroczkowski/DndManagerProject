using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DndManager.Accessor
{
    public class Accessor<T>
    {
        public T? Value { get; set; }

        public Accessor()
        {
        }

        public Accessor(T value)
        {
            Value = value;
        }
    }
}
