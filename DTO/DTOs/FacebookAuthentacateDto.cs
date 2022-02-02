using Newtonsoft.Json;

namespace DTO
{
    public class FacebookAuthentacateDto
    {
        [JsonProperty("id")]
        public string FacebookId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}