using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungGuns.Data.Contracts
{
    public class AdjacencyListItem : TableEntity
    {
        public List<uint> DependentFields { get; set; }
    }
}
