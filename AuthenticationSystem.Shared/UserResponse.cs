using Newtonsoft.Json;
using System.Data;
using System.Text.Json.Serialization;

namespace AuthenticationSystem.Shared
{
    public class UserResponse
    {
        [JsonProperty("Name")]
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonProperty("Surname")]
        [JsonPropertyName("Surname")]
        public string Surname { get; set; }

        [JsonProperty("Id")]
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [JsonProperty("PhoneNumber")]
        [JsonPropertyName("PhoneNumber")]
        public int? PhoneNumber { get; set; }

        [JsonProperty("BirthDate")]
        [JsonPropertyName("BirthDate")]
        public DateOnly? BirthDate { get; set; }

        [JsonProperty("Role")]
        [JsonPropertyName("Role")]
        public RoleResponse Role { get; set; }
    }
}
