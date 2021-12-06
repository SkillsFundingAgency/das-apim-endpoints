using System;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.InnerApi.Requests
{
    public class PutCreateUserRequest : IPutApiRequest
    {
        private readonly Guid _id;

        public PutCreateUserRequest(Guid id, PutCreateUserRequestData data)
        {
            _id = id;
            Data = data;
        }

        public string PutUrl => $"api/users/{_id}";
        public object Data { get; set; }
    }

    public class PutCreateUserRequestData
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
        [JsonProperty("state")]
        public readonly int State = 0;
    }
}