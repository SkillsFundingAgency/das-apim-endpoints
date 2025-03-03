using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests
{
    public class GetKsbsByApprenticeshipIdQueryRequest : IGetApiRequest
    {
        public long ApprenticeshipId;

        public GetKsbsByApprenticeshipIdQueryRequest(long apprenticeshipId)
        {
            ApprenticeshipId = apprenticeshipId;
        }

        public string GetUrl => $"apprenticeships/{ApprenticeshipId}/ksbs";
    }
}
