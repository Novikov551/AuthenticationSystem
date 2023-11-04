using AuthenticationSystem.Domain.Core;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AuthenticationSystem.Shared
{
    public class RoleResponse
    {
        [JsonProperty("RoleId")]
        [JsonPropertyName("RoleId")]
        public Guid RoleId { get; set; }

        [JsonProperty("RoleType")]
        [JsonPropertyName("RoleType")]
        public RoleType RoleType { get; set; }
    }
}
