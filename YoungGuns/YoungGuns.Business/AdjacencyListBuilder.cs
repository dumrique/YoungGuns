using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using YoungGuns.Shared;
using YoungGuns.Data;
using System.Threading.Tasks;

namespace YoungGuns.Business
{
    public class AdjacencyListBuilder
    {

        public static void ExtractAndStoreAdjacencyList(TaxSystemDto dto)
        {
            Dictionary<uint, List<uint>> adjListsInverse = new Dictionary<uint, List<uint>>();
            foreach (FieldDto field in dto.taxsystem_fields)
                adjListsInverse[field.field_id] = ExtractFieldsFromFormula(field.field_calculation);
           
            // we need the inverse of the list above for storage
            //      i.e., we need the list of dependent fields for each field
            foreach (FieldDto field in leafNodes)
            {
                foreach (uint depId in adjListsInverse[field.field_id].ToList())
                    BuildAdjListWorker(depId, adjListsInverse, adjListsInverse[field.field_id]);

                await DAGUtilities.StoreLeafAdjacencyListAsync(field.field_id, dto.taxsystem_name, adjListsInverse[field.field_id]);
            }
        }

        private static List<uint> ExtractFieldsFromFormula(string field_calculation)
        {
            List<uint> formulaFields = new List<uint>();
            foreach (Match m in Regex.Matches(field_calculation, "\\[(.*)\\]"))
                formulaFields.Add(uint.Parse(m.Value));

            return formulaFields;
        }

        private static void BuildAdjListWorker(uint id, Dictionary<uint, List<uint>> masterAdjList, List<uint> leafAdjList)
        {
            List<uint> masterList = masterAdjList.ContainsKey(id) ? masterAdjList[id] : null;
            if (masterList != null && masterList.Count > 0)
            {
                foreach (uint nextId in masterList)
                {
                    if (!leafAdjList.Contains(nextId))
                        leafAdjList.Add(nextId);
                }

                foreach (uint nextNextId in masterList)
                    BuildAdjListWorker(nextNextId, masterAdjList, leafAdjList);
            }
        }
    }
}
