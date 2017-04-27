using Newtonsoft.Json;
using System.Collections.Generic;

namespace YoungGuns.Shared
{
    public class TaxSystemTopoFieldList
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("topoList")]
        public List<uint> TopoList { get; set; }
    }
}
