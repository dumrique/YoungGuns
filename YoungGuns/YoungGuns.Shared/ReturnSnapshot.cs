using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungGuns.Shared
{
    public class ReturnSnapshot
    {
        [JsonProperty("version")]
        public int Version { get; set; }
        [JsonProperty("changesetFields")]
        public List<uint> ChangesetFields { get; set; }
        [JsonProperty("fieldValues")]
        public Dictionary<uint,object> FieldValues { get; set; }
    }
}
