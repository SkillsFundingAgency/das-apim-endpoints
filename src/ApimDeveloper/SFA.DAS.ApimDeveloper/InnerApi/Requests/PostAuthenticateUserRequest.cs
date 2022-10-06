using System;
using System.Text.Json.Serialization;
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
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        
    }

    public class PostAuthenticateUserResult
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
        [JsonPropertyName("authenticated")]
        public bool Authenticated { get; set; }
    }
}