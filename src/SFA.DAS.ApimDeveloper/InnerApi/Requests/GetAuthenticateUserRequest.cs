using System;
using System.Web;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.InnerApi.Requests
{
    public class GetAuthenticateUserRequest: IGetApiRequest
    {
        private readonly string _email;
        private readonly string _password;

        public GetAuthenticateUserRequest(string email, string password)
        {
            _email = HttpUtility.UrlEncode(email);
            _password = HttpUtility.UrlEncode(password);
        }

        public string GetUrl => $"api/users/authenticate?email={_email}&password={_password}";
    }

    public class GetAuthenticateUserResult
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("authenticated")]
        public bool Authenticated { get; set; }
    }
}