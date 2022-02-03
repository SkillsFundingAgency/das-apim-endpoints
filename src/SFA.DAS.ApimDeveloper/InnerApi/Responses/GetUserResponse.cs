using System;
using Newtonsoft.Json;

namespace SFA.DAS.ApimDeveloper.InnerApi.Responses
{
    public class GetUserResponse
    {
        [JsonProperty("firstName")] public string FirstName { get; set; }
        [JsonProperty("lastName")] public string LastName { get; set; }
        [JsonProperty("email")] public string Email { get; set; }
        [JsonProperty("id")] public Guid Id { get; set; }
        [JsonProperty("state")] public string State { get; set; }
    }
}