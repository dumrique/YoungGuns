using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;


namespace YoungGuns.Data.Contracts
{
    public class AdjacencyListItem : TableEntity
    {
        public List<uint> DependentFields { get; set; }
    }

}

