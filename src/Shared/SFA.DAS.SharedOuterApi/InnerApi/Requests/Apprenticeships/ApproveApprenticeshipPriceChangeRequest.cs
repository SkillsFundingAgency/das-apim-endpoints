using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships
{
    public class PatchApproveApprenticeshipPriceChangeRequest : IPatchApiRequest<ApproveApprenticeshipPriceChangeRequest>
    {
        public PatchApproveApprenticeshipPriceChangeRequest(
            Guid apprenticeshipKey,
            string userId)
        {
            ApprenticeshipKey = apprenticeshipKey;
            Data = new ApproveApprenticeshipPriceChangeRequest
            {
                UserId = userId
            };
        }
        
        public Guid ApprenticeshipKey { get; set; }
        public string PatchUrl => $"{ApprenticeshipKey}/priceHistory/pending";
        public ApproveApprenticeshipPriceChangeRequest Data { get; set; }
    }

    public class ApproveApprenticeshipPriceChangeRequest
    {
        public string UserId { get; set; }
    }
}