using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class PutAccountUserRequest : IPutApiRequest
    {
        public PutAccountUserRequest(string userRef, string email, string firstName, string lastName, Guid? correlationId)
        {
            Data = new
            {
                UserRef = userRef,
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                CorrelationId = correlationId.HasValue ? correlationId.Value.ToString() : string.Empty
            };
        }

        public string PutUrl => $"api/user/upsert";
        public object Data { get; set; }
    }
}