using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungGuns.Shared
{
    public abstract class FieldBase<T>
    {
        public uint Id { get; set; }
        public string Description { get; set; }
        public T Value { get; set; }
    }
}
