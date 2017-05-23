using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
                DependentFields = JsonConvert.SerializeObject(dependencyList)
            };

            // Create the TableOperation object that inserts the adjacency list item.
            TableQuery<AdjacencyListItem> query = new TableQuery<AdjacencyListItem>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "leaf"))
                .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, leafFieldId.ToString()));

            var item = table.ExecuteQuery(query).FirstOrDefault();
            if (item != null)
            {
                TableOperation delOperation = TableOperation.Delete(item);
                await table.ExecuteAsync(delOperation);
            }

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
                DependentFields = JsonConvert.SerializeObject(depList)
            };

            // Create the TableOperation object that inserts the adjacency list item.
            TableQuery<AdjacencyListItem> query = new TableQuery<AdjacencyListItem>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "calc"))
                .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, field_id.ToString()));

            var item = table.ExecuteQuery(query).FirstOrDefault();
            if(item != null)
            {
                TableOperation delOperation = TableOperation.Delete(item);
                await table.ExecuteAsync(delOperation);
            }

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
            var calcNodes = request.taxsystem_fields.Where(f => f.field_type == "calcformula").ToList();

            foreach(var node in calcNodes)
            {
                var r = request.taxsystem_fields.FirstOrDefault(f => f.field_id == node.field_id);
                foreach (Match m in Regex.Matches(node.field_calculation, "\\[.*?\\]"))
                {
                    r.field_calculation = r.field_calculation.Replace(m.Value, $"[{reverseFieldLookup[m.Value].ToString()}]");
                }
            }
        }

        public static async Task<CloudTable> GetAdjacencyListTable(string taxSystemName)
        {
            CloudTable table = GetAdjacencyTableListTableReference(taxSystemName);

            // Create the table if it doesn't exist.
            try
            {
                await table.CreateIfNotExistsAsync();
            }
            catch (Exception e)
            {
                
            }

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
                GetConnectionString());

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            return tableClient.GetTableReference($"TaxSystemAdjacencyLists{taxSystemName}");
        }

        private static string GetConnectionString()
        {
            //return "DefaultEndpointsProtocol=https;AccountName=youngguns;AccountKey=NGct+PJexXQ0Eby6DhuOQ555dev7V6Z+lciJyunYM6aXVoEvzQD8Ig2FVv5YGiklTlPLaUENU4Cgg4N2pFzY2A==;EndpointSuffix=core.windows.net";
            return "UseDevelopmentStorage=true";
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
