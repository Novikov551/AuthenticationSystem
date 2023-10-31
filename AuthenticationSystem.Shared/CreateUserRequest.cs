using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AuthenticationSystem.Shared
{
    public class CreateUserRequest
    {
        [JsonProperty("Name")]
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonProperty("Surname")]
        [JsonPropertyName("Surname")]
        public string Surname { get; set; }

        [JsonProperty("Email")]
        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [JsonProperty("Password")]
        [JsonPropertyName("Password")]
        public string Password { get; set; }
    }
}
