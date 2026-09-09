using System;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

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