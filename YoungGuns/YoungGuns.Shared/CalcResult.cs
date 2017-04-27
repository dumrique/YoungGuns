using Newtonsoft.Json;

namespace YoungGuns.Shared
{
    public class CalcResult
    {
        [JsonProperty("mergeResult")]
        public bool? MergeResult { get; set; }
        [JsonProperty("returnSnapshot")]
        public ReturnSnapshot ReturnSnapshot { get; set; }
    }
}
