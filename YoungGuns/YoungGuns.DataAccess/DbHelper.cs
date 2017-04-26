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
            return "DefaultEndpointsProtocol=https;AccountName=youngguns;AccountKey=NGct+PJexXQ0Eby6DhuOQ555dev7V6Z+lciJyunYM6aXVoEvzQD8Ig2FVv5YGiklTlPLaUENU4Cgg4N2pFzY2A==;EndpointSuffix=core.windows.net";
        }
    }
}
