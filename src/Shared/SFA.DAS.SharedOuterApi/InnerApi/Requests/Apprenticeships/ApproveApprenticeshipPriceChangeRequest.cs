using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships
{
    public class PatchApproveApprenticeshipPriceChangeRequest : IPatchApiRequest<ApproveApprenticeshipPriceChangeRequest>
    {
        public PatchApproveApprenticeshipPriceChangeRequest(
            Guid apprenticeshipKey,
            string userId,
            decimal? TrainingPrice,
            decimal? AssessmentPrice)
        {
            ApprenticeshipKey = apprenticeshipKey;
            Data = new ApproveApprenticeshipPriceChangeRequest
            {
                UserId = userId,
                TrainingPrice = TrainingPrice,
                AssessmentPrice = AssessmentPrice
            };
        }
        
        public Guid ApprenticeshipKey { get; set; }
        public string PatchUrl => $"{ApprenticeshipKey}/priceHistory/pending";
        public ApproveApprenticeshipPriceChangeRequest Data { get; set; }
    }

    public class ApproveApprenticeshipPriceChangeRequest
    {
        public string UserId { get; set; }
        // These 2 properties are only used when a provider is approving a employer initiated price change
        public decimal? TrainingPrice { get; set; }
        public decimal? AssessmentPrice { get; set; }
    }
}