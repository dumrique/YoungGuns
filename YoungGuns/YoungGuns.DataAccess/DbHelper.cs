using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YoungGuns.Data;
using YoungGuns.Shared;
using Microsoft.Azure.Documents.Client;
using System.Linq;

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

        public CalcDAG GetCalcDag(string taxSystemId)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(CalcDAG).Name);
            var collection = _client.CreateDocumentQuery<CalcDAG>(uri);
            var result = collection.Where(item => item.TaxSystem.Id.Equals(taxSystemId)).ToList();
            var dag = result.First();
            return dag;
        }

        public async Task UpsertCalcDag(CalcDAG dag)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(CalcDAG).Name);
            var result = await _client.UpsertDocumentAsync(uri, dag);
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
