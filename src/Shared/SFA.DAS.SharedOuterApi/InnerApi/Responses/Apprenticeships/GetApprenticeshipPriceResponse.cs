using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships
{
    public class GetApprenticeshipPriceResponse
    {
        public Guid ApprenticeshipKey { get; set; }
        public decimal FundingBandMaximum { get; set; }
        public decimal TrainingPrice { get; set; }
        public decimal AssessmentPrice { get; set; }
        public DateTime EarliestEffectiveDate { get; set; }
    }
}