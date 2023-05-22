using SFA.DAS.Approvals.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests
{
    public class GetApprenticeshipUpdatesRequest : IGetApiRequest
    {
        public readonly long ApprenticeshipId;
        public byte Status { get; set; }

        public GetApprenticeshipUpdatesRequest(long apprenticeshipId, ApprenticeshipStatus status)
        {
            ApprenticeshipId = apprenticeshipId;
            Status = Convert.ToByte(status);
        }

        public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}/updates?Status={Status}";
    }
}
