using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class GetMyApprenticeshipRequest : IGetApiRequest
    {
        private readonly Guid _apprenticeId;

        public GetMyApprenticeshipRequest(Guid apprenticeId)
        {
            _apprenticeId = apprenticeId;
        }

        public string GetUrl => $"apprentices/{_apprenticeId}/myapprenticeship";
    }
}
