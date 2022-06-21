using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class DeleteShortlistForUserRequest : IDeleteApiRequest
    {
        private readonly Guid _userId;

        public DeleteShortlistForUserRequest(Guid userId)
        {
            _userId = userId;
        }

        public string DeleteUrl => $"api/shortlist/users/{_userId}";
    }
}