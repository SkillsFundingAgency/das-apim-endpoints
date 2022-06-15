using System;
using System.Web;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.InnerApi.Requests
{
    public class PostAuthenticateUserRequest: IPostApiRequest
    {
        
        public PostAuthenticateUserRequest(string email, string password)
        {
            Data = new PostAuthenticateUserRequestData
            {
                Email = email,
                Password = password
            };
        }
        
        public string PostUrl => "api/users/authenticate";
        public object Data { get; set; }
    }

    public class PostAuthenticateUserRequestData
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        
    }

    public class PostAuthenticateUserResult
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