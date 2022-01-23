using Newtonsoft.Json;

namespace SFA.DAS.ApimDeveloper.InnerApi.Requests
{
    public class UserRequestData
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("confirmEmailLink")]
        public string ConfirmationEmailLink { get; set; }
        [JsonProperty("state", Required = Required.AllowNull)]
        public int? State { get; set; }
    }
}