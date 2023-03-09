using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class PutUpsertEmployerUserAccountRequest : IPutApiRequest
    {
        private readonly Guid _userId;

        public PutUpsertEmployerUserAccountRequest(Guid userId, string id, string email, string firstName, string lastName)
        {
            _userId = userId;
            Data = new
            {
                GovUkIdentifier = id,
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };
        }


        public string PutUrl => $"api/users/{_userId}";
        public object Data { get; set; }
    }
}