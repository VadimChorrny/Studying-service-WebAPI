using Newtonsoft.Json;

namespace DTO
{
    public class FacebookUserInfoDto
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        
        [JsonProperty("email")]
        public  string Email { get; set; }
    }
}