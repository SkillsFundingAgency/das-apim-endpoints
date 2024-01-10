using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships
{
    public class PatchRejectApprenticeshipPriceChangeRequest : IPatchApiRequest<RejectApprenticeshipPriceChangeRequest>
    {
        public PatchRejectApprenticeshipPriceChangeRequest(
            Guid apprenticeshipKey,
            string reason)
        {
            ApprenticeshipKey = apprenticeshipKey;
            Data = new RejectApprenticeshipPriceChangeRequest
            {
                Reason = reason
            };
        }
        
        public Guid ApprenticeshipKey { get; set; }
        public string PatchUrl => $"{ApprenticeshipKey}/priceHistory";
        public RejectApprenticeshipPriceChangeRequest Data { get; set; }
    }

    public class RejectApprenticeshipPriceChangeRequest
    {
        public string Reason { get; set; }
    }
}