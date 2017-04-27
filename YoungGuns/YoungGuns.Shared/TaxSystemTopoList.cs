using Newtonsoft.Json;
using System.Collections.Generic;

namespace YoungGuns.Shared
{
    public class TaxSystemTopoList
    {
        [JsonProperty("taxSystemId")]
        public string TaxSystemId { get; set; }

        [JsonProperty("topoList")]
        public List<uint> TopoList { get; set; }
    }
}
