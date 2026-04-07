using Models.PetService.General;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.PetService.Payload
{
    public class PostPetPayload
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