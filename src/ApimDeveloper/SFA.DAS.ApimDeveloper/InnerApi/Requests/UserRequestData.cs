using System.Text.Json.Serialization;

namespace SFA.DAS.ApimDeveloper.InnerApi.Requests
{
    public class UserRequestData
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("confirmEmailLink")]
        public string ConfirmationEmailLink { get; set; }
        [JsonPropertyName("state")]
        public int? State { get; set; }
    }
}