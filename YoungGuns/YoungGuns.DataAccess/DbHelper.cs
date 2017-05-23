using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YoungGuns.Shared;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using YoungGuns.DataAccess.Contracts;
using Newtonsoft.Json;

namespace YoungGuns.DataAccess
{
    public class DbHelper
    {
        private const string DatabaseName = "YoungGuns";
        private readonly DocumentClient _client;

        public DbHelper()
        {
            _client = new DocumentClient(GetEndpointUrl(), GetPrimaryKey());
            // Connect to the Azure Cosmos DB Emulator running locally
            //_client = new DocumentClient(
            //    new Uri("https://localhost:8081"),
            //    "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
        }

        public TaxSystem GetTaxSystem(string id)
        {
            Uri uri = UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TaxSystem).Name);
            var collection = _client.CreateDocumentQuery<TaxSystem>(uri);
            List<TaxSystem> result = collection.Where(item => item.Id.Equals(id)).ToList();
            TaxSystem taxSystem = result.FirstOrDefault();
            return taxSystem;
        }

        public List<TaxSystem> GetAllTaxSystems()
        {
            var uri = UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TaxSystem).Name);
            var collection = _client.CreateDocumentQuery<TaxSystem>(uri);
            return collection.ToList();
        }

        public async Task<string> UpsertTaxSystem(TaxSystem system)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TaxSystem).Name);
            var result = await _client.UpsertDocumentAsync(uri, system);
            return result.Resource.Id;
        }

        public static async Task<List<uint>> GetDependentFields(string taxSystem, uint fieldId)
        {
            CloudTable table = await DAGUtilities.GetAdjacencyListTable(taxSystem);
            // Construct the query operation for the field list for the given field
            TableOperation retrieveOperation = TableOperation.Retrieve<AdjacencyListItem>("leaf", fieldId.ToString());

            // Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);
            return JsonConvert.DeserializeObject<List<uint>>(((AdjacencyListItem)retrievedResult.Result)?.DependentFields);
        }

        public List<uint> GetTopoList(string taxSystemId)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TaxSystem).Name);
            var query = _client.CreateDocumentQuery<TaxSystem>(uri);
            List<TaxSystem> result = query.Where(item => item.Id.Equals(taxSystemId)).ToList();
            return result.FirstOrDefault()?.TopoList;
        }

        public async Task SaveTopoList(TaxSystemTopoList topoList)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TaxSystem).Name);
            var query = _client.CreateDocumentQuery<TaxSystem>(uri);
            TaxSystem result = query.Where(item => item.Id.Equals(topoList.TaxSystemId)).ToList().FirstOrDefault();

            result.TopoList = topoList.TopoList;

            //var uri = UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TaxSystemTopoList).Name);

            // add the TopoList to the TaxSystem object
            await _client.UpsertDocumentAsync(uri, result);
        }

        public List<uint> GetReturnChangesetFields(string returnId, uint returnVersion)
        {
            return GetReturnSnapshot(returnId, returnVersion)?.ChangesetFields;
        }

        public ReturnSnapshot GetReturnSnapshot(string returnId, uint returnVersion)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TaxSystem).Name);
            var query = _client.CreateDocumentQuery<ReturnSnapshot>(uri);
            return query.FirstOrDefault(item => item.ReturnId.Equals(returnId) && item.Version.Equals(returnVersion));
        }

        public async Task<string> CreateReturnSnapshot(ReturnSnapshot returnSnapshot)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(ReturnSnapshot).Name);
            var result = await _client.CreateDocumentAsync(uri, returnSnapshot);
            return result.Resource.Id;
        }

        public static async Task<Dictionary<uint, List<uint>>> GetCalcAdjacencyList(string taxSystemName)
        {
            CloudTable table = await DAGUtilities.GetAdjacencyListTable(taxSystemName);

            TableQuery<AdjacencyListItem> query = new TableQuery<AdjacencyListItem>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "calc"));

            var listItems = table.ExecuteQuery(query).ToList();
            var dict = new Dictionary<uint, List<uint>>();
            foreach (var li in listItems)
            {
                uint id;
                if (uint.TryParse(li.RowKey, out id))
                    dict[id] = JsonConvert.DeserializeObject<List<uint>>(li.DependentFields);
                else
                    throw new Exception($"Invalid Field Id: {li.RowKey}");
            }
            return dict;
        }
        
        public string GetConnectionString()
        {
            return "AccountEndpoint=https://youngguns.documents.azure.com:443/;AccountKey=F7a8aOEM1XHZLkkxJBjY9gyAMM5kjWxj1mNgIYxN2DU409oV3NoNEVpEzpwqTc6PPK6ZXWhGHZI6hqgCSjsgtA==;";
        }

        #region Private
        private Uri GetEndpointUrl()
        {
            return new Uri("https://youngguns.documents.azure.com:443/");
        }

        private string GetPrimaryKey()
        {
            return "MvSSeNy98O4nBUr5AUvu7rEt9C1iBKICPfb6q6Qttr5f1nn8pbFzP7PUwu3ycs2HwE7jlqNtaEYXuwjBljMvzw==";
        }
        #endregion
    }
}
