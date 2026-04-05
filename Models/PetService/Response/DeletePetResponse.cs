using Newtonsoft.Json;

namespace Models.PetService.Response
{
    public class DeletePetResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }
    }
}