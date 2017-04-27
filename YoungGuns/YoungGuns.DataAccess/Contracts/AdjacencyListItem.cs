using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;


namespace YoungGuns.DataAccess.Contracts
{
    public class AdjacencyListItem : TableEntity
    {
        public List<uint> DependentFields { get; set; }
    }

}

