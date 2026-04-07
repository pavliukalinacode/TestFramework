using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.PetService.General
{
    public class PetResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("category")]
        public Category? Category { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("photoUrls")]
        public List<string>? PhotoUrls { get; set; }

        [JsonProperty("tags")]
        public List<Tag>? Tags { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }
    }
}