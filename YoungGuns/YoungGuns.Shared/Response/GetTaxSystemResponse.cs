using Newtonsoft.Json;
using System.Collections.Generic;

namespace YoungGuns.Shared.Response
{
    public class GetTaxSystemResponse
    {
        [JsonProperty("taxsystem_id")]
        public string Id { get; set; }
        [JsonProperty("taxsystem_name")]
        public string Name { get; set; }
        [JsonProperty("taxsystem_fields")]
        public IEnumerable<FieldDto> Fields { get; set; }
    }
}
