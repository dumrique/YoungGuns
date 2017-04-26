using NCalc;
using YoungGuns.Shared;
using System.Collections.Generic;

namespace YoungGuns.Data
{
    /// <summary>
    /// Represents a field that can be calced in a TaxSystem and has a float value.
    /// </summary>
    public class CalcField : FieldBase<float>
    {
        public Expression Formula { get; set; }
        public List<uint> ParameterFields { get; set; }


    }
}
