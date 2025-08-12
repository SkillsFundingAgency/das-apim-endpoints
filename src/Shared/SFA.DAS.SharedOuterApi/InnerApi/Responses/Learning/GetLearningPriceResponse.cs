﻿using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning
{
    public class GetLearningPriceResponse
    {
        public Guid ApprenticeshipKey { get; set; }
        public int FundingBandMaximum { get; set; }
        public decimal TrainingPrice { get; set; }
        public decimal AssessmentPrice { get; set; }
        public DateTime? ApprenticeshipActualStartDate { get; set; }
        public DateTime? ApprenticeshipPlannedEndDate { get; set; }
        public long? AccountLegalEntityId { get; set; }
        public long UKPRN { get; set; }
    }
}