using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetApprenticeRequest : IGetApiRequest
    {
        public Guid ApprenticeId { get; }

        public GetApprenticeRequest(Guid apprenticeId)
        {
            ApprenticeId = apprenticeId;
        }
        public string GetUrl => $"apprentices/{ApprenticeId}";
    }
}