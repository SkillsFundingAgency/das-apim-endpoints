using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships
{
    public class ApprenticeshipPrice
    {
        public Guid ApprenticeshipKey { get; set; }
        public decimal? TrainingPrice { get; set; }
        public decimal? AssessmentPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal FundingBandMaximum { get; set; }
        public DateTime EarliestEffectiveDate { get; set; }
    }
}
