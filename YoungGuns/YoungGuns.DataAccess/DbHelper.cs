using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YoungGuns.Data;
using YoungGuns.Shared;
using Microsoft.Azure.Documents.Client;

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
            return null;
        }

        public List<TaxSystem> GetAllTaxSystems()
        {
            return null;
        }

        public async Task<string> InsertTaxSystem(TaxSystem system)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(DatabaseName, typeof(TaxSystem).Name);
            var result = await _client.UpsertDocumentAsync(uri, system);
            return result.Resource.Id;
        }

        public CalcDAG GetCalcDag(string taxSystemId)
        {
            return null;
        }

        public void InsertCalcDag(CalcDAG dag)
        {
            
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
