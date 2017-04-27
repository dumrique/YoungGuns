using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoungGuns.Data.Contracts;
using YoungGuns.Shared;

namespace YoungGuns.Data
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


    }
}
