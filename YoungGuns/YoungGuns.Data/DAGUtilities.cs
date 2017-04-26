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
        public static void TopologicalSort() { }

        public static async Task StoreLeafAdjacencyListAsync(uint leafFieldId, List<uint> dependencyList)
        {
            CloudTable table = await GetAdjacencyListTable();

            AdjacencyListItem adjLI = new AdjacencyListItem()
            {
                DependentFields = new List<uint>(dependencyList)
            };

            // Create the TableOperation object that inserts the adjacency list item.
            TableOperation insertOperation = TableOperation.Insert(adjLI);

            // Execute the insert operation.
            await table.ExecuteAsync(insertOperation);
        }

        public async static Task<CloudTable> GetAdjacencyListTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                "DefaultEndpointsProtocol=https;AccountName=youngguns;AccountKey=NGct+PJexXQ0Eby6DhuOQ555dev7V6Z+lciJyunYM6aXVoEvzQD8Ig2FVv5YGiklTlPLaUENU4Cgg4N2pFzY2A==;EndpointSuffix=core.windows.net");

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("TaxSystemAdjacencyLists");

            // Create the table if it doesn't exist.
            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}
