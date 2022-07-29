using System;
using System.Text.Json.Serialization;

namespace SFA.DAS.ApimDeveloper.InnerApi.Responses
{
    public class GetUserResponse
    {
        [JsonPropertyName("firstName")] 
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")] 
        public string LastName { get; set; }
        [JsonPropertyName("email")] 
        public string Email { get; set; }
        [JsonPropertyName("id")] 
        public Guid Id { get; set; }
        [JsonPropertyName("state")] 
        public string State { get; set; }
    }
}