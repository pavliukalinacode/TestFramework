using Newtonsoft.Json;

namespace Models.PetService.General
{
    public class Tag
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}