using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class DeleteShortlistItemForUserRequest : IDeleteApiRequest
    {
        private readonly Guid _id;
        private readonly Guid _userId;

        public DeleteShortlistItemForUserRequest(Guid id, Guid userId)
        {
            _id = id;
            _userId = userId;
        }

        public string DeleteUrl => $"api/shortlist/users/{_userId}/items/{_id}";
    }
}