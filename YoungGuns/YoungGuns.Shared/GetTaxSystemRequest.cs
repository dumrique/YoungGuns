using Newtonsoft.Json;

namespace YoungGuns.Shared
{
    public class GetTaxSystemRequest
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
