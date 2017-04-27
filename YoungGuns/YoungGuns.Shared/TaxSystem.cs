﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace YoungGuns.Shared
{
    public class TaxSystem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("fields")]
        public IEnumerable<FieldDto> Fields { get; set; }
    }
}
