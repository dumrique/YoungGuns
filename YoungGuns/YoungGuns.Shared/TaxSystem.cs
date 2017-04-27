using Newtonsoft.Json;

namespace YoungGuns.Shared
{
    public class TaxSystem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
