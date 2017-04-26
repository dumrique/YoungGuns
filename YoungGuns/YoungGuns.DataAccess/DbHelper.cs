using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoungGuns.Data;
using YoungGuns.Shared;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace YoungGuns.DataAccess
{
    public class DbHelper
    {
        public TaxSystem GetTaxSystem(string id)
        {
            return null;
        }

        public List<TaxSystem> GetAllTaxSystems()
        {
            return null;
        }

        public void InsertTaxSystem(TaxSystem system)
        {
            
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

        public string GetEndpointUrl()
        {
            return "https://youngguns.documents.azure.com:443/";
        }

        public string GetPrimaryKey()
        {
            return "F7a8aOEM1XHZLkkxJBjY9gyAMM5kjWxj1mNgIYxN2DU409oV3NoNEVpEzpwqTc6PPK6ZXWhGHZI6hqgCSjsgtA==";
        }
    }
}
