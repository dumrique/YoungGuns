using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using YoungGuns.Shared;
using YoungGuns.Data;
using System.Threading.Tasks;

namespace YoungGuns.Business
{
    public class AdjacencyListBuilder
    {
        public async static Task ExtractAndStoreAdjacencyList(TaxSystemDto dto)
        {
            Dictionary<uint, List<uint>> adjListsInverse = new Dictionary<uint, List<uint>>();
            var leafNodes = dto.taxsystem_fields.Where(f => f.field_type == "calcfield").ToList();

            foreach (FieldDto field in dto.taxsystem_fields)
            {
                foreach (uint id in ExtractFieldsFromFormula(field.field_calculation))
                {
                    if (adjListsInverse[id].Contains(field.field_id))
                        adjListsInverse[id].Add(field.field_id);
                }
            }

            // we need the inverse of the list above for storage
            //      i.e., we need the list of dependent fields for each field
            foreach (FieldDto field in leafNodes)
            {
                foreach (uint depId in adjListsInverse[field.field_id].ToList())
                    BuildAdjListWorker(depId, adjListsInverse, adjListsInverse[field.field_id]);

                await DAGUtilities.StoreLeafAdjacencyListAsync(field.field_id, adjListsInverse[field.field_id]);
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
