using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using YoungGuns.Shared;

namespace YoungGuns.Business
{
    public class AdjacencyListBuilder
    {

        public static void ExtractAndStoreAdjacencyList(TaxSystemDto dto)
        {
            Dictionary<uint, List<uint>> adjListsInverse = new Dictionary<uint, List<uint>>();
            foreach (FieldDto field in dto.taxsystem_fields)
                adjListsInverse[field.field_id] = ExtractFieldsFromFormula(field.field_calculation);
           

        }

        private static List<uint> ExtractFieldsFromFormula(string field_calculation)
        {
            List<uint> formulaFields = new List<uint>();
            foreach (Match m in Regex.Matches(field_calculation, "\\[(.*)\\]"))
                formulaFields.Add(uint.Parse(m.Value));

            return formulaFields;
        }

        private static List<uint> BuildAdjListWorker(List<FieldDto> allFields, object field_id)
        {
            return null;
        }
    }
}
