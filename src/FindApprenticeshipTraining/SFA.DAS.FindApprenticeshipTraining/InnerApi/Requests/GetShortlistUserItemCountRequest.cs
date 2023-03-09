using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetShortlistUserItemCountRequest : IGetApiRequest
    {
        private readonly Guid _userId;

        public GetShortlistUserItemCountRequest(Guid userId)
        {
            _userId = userId;
        }

        public string GetUrl => $"api/Shortlist/users/{_userId}/count";
    }
}