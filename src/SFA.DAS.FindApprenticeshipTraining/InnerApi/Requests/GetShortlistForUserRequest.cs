using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetShortlistForUserRequest : IGetApiRequest
    {
        private readonly Guid _shortlistUserId;
        public GetShortlistForUserRequest(Guid shortlistUserId)
        {
            _shortlistUserId = shortlistUserId;
        }

        public Guid ShortlistUserId => _shortlistUserId;

        public string GetUrl => $"api/shortlist/users/{_shortlistUserId}";
    }
}