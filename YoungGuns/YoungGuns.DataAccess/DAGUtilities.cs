using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YoungGuns.DataAccess.Contracts;
using YoungGuns.Shared;

namespace YoungGuns.DataAccess
{
    public class DAGUtilities
    {
        public static async Task StoreLeafAdjacencyListAsync(uint leafFieldId, string taxSystemName, List<uint> dependencyList)
        {
            CloudTable table = await GetAdjacencyListTable(taxSystemName);

            AdjacencyListItem adjLI = new AdjacencyListItem()
            {
                PartitionKey = "leaf",
                RowKey = leafFieldId.ToString(),
                DependentFields = new List<uint>(dependencyList)
            };

            // Create the TableOperation object that inserts the adjacency list item.
            TableOperation insertOperation = TableOperation.Insert(adjLI);

            // Execute the insert operation.
            await table.ExecuteAsync(insertOperation);
        }

        public static async Task StoreCalcAdjacencyListAsync(uint field_id, string taxSystemName, List<uint> depList)
        {
            CloudTable table = await GetAdjacencyListTable(taxSystemName);

            AdjacencyListItem adjLI = new AdjacencyListItem()
            {
                PartitionKey = "calc",
                RowKey = field_id.ToString(),
                DependentFields = new List<uint>(depList)
            };

            // Create the TableOperation object that inserts the adjacency list item.
            TableOperation insertOperation = TableOperation.Insert(adjLI);

            // Execute the insert operation.
            await table.ExecuteAsync(insertOperation);
        }

        public static void FixCalcFormulaMappings(PostTaxSystemRequest request)
        {
            // build lookup table
            Dictionary<string, uint> reverseFieldLookup = new Dictionary<string, uint>();
            foreach(var field in request.taxsystem_fields)
                reverseFieldLookup[$"[{field.field_title}]"] = field.field_id;

            for(int i = 0; i < request.taxsystem_fields.Count; i++)
            {
                foreach (Match m in Regex.Matches(request.taxsystem_fields[i].field_calculation, "\\[.*?\\]"))
                {
                    request.taxsystem_fields[i].field_calculation =
                        request.taxsystem_fields[i].field_calculation.Replace(m.Value, reverseFieldLookup[m.Value].ToString());
                }
            }
        }

        public static async Task<CloudTable> GetAdjacencyListTable(string taxSystemName)
        {
            CloudTable table = GetAdjacencyTableListTableReference(taxSystemName);

            // Create the table if it doesn't exist.
            await table.CreateIfNotExistsAsync();

            return table;
        }

        public static Task<bool> DeleteAdjacencyListTable(string taxSystemName)
        {
            CloudTable table = GetAdjacencyTableListTableReference(taxSystemName);

            return table.DeleteIfExistsAsync();
        }

        public static CloudTable GetAdjacencyTableListTableReference(string taxSystemName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                "DefaultEndpointsProtocol=https;AccountName=youngguns;AccountKey=NGct+PJexXQ0Eby6DhuOQ555dev7V6Z+lciJyunYM6aXVoEvzQD8Ig2FVv5YGiklTlPLaUENU4Cgg4N2pFzY2A==;EndpointSuffix=core.windows.net");

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            return tableClient.GetTableReference($"TaxSystemAdjacencyLists_{taxSystemName}");
        }

        /// <summary>
        /// Returns true if the 2 changesets have a potential merge conflict.
        /// </summary>
        /// <param name="changeset1"></param>
        /// <param name="changeset2"></param>
        /// <returns></returns>
        public static async Task<bool> CheckForMergeConflicts(string taxSystemName, List<uint> changeset1Fields, List<uint> changeset2Fields)
        {
            List<uint> calcFields1 = new List<uint>();
            List<uint> calcFields2 = new List<uint>();

            // get list of fields to calc in DAG from each changeset
            foreach (var key in changeset1Fields)
                calcFields1.AddRange(await DbHelper.GetDependentFields(taxSystemName, key));
            foreach (var key in changeset2Fields)
                calcFields2.AddRange(await DbHelper.GetDependentFields(taxSystemName, key));

            // compare lists to find commonality
            return calcFields1.Intersect(calcFields2).Any();
        }
    }
}
