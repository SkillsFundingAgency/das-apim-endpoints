using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.InnerApi.Requests
{
    public class PostCreateUserRequest : IPostApiRequest
    {
        private readonly Guid _id;
        public PostCreateUserRequest (Guid id, UserRequestData data)
        {
            _id = id;
            Data = data;
        }
        public string PostUrl  => $"api/users/{_id}";
        public object Data { get; set; }
    }
}