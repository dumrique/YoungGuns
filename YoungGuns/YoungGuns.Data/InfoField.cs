using YoungGuns.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungGuns.Data
{
    /// <summary>
    /// Represents a field in a TaxSystem.
    /// </summary>
    /// <typeparam name="T">Type of this field's value.</typeparam>
    public class InfoField : FieldBase<string>
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public TaxSystem TaxSystem { get; set; }
    }
}
