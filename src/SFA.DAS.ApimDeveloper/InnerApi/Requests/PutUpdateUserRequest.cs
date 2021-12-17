using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.InnerApi.Requests
{
    public class PutUpdateUserRequest : IPutApiRequest
    {
        private readonly Guid _id;

        public PutUpdateUserRequest(Guid id, UserRequestData data)
        {
            _id = id;
            Data = data;
        }

        public string PutUrl => $"api/users/{_id}";
        public object Data { get; set; }
    }
}