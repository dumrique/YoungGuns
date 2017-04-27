using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YoungGuns.Shared;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using YoungGuns.DataAccess.Contracts;

namespace YoungGuns.DataAccess
{
    public class DbHelper
    {
        private const string DatabaseName = "YoungGuns";
        private readonly DocumentClient _client;

        public DbHelper()
        {
            _client = new DocumentClient(GetEndpointUrl(), GetPrimaryKey());
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
            return ((AdjacencyListItem)retrievedResult.Result)?.DependentFields;
        }

        public List<uint> GetTopoList(string taxSystemId)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TaxSystem).Name);
            var query = _client.CreateDocumentQuery<TaxSystemTopoFieldList>(uri);
            List<TaxSystemTopoFieldList> result = query.Where(item => item.Id.Equals(taxSystemId)).ToList();
            return result.FirstOrDefault()?.TopoList;
        }

        public async Task SaveTopoList(TaxSystemTopoFieldList topoList)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TaxSystem).Name);

            // add the TopoList to the TaxSystem object
            var result = await _client.UpsertDocumentAsync(uri, topoList);
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
                    dict[id] = li.DependentFields;
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
            return "F7a8aOEM1XHZLkkxJBjY9gyAMM5kjWxj1mNgIYxN2DU409oV3NoNEVpEzpwqTc6PPK6ZXWhGHZI6hqgCSjsgtA==";
        }
        #endregion
    }
}
