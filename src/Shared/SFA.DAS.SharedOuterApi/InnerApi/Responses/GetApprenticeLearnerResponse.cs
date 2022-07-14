using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public  class GetApprenticeLearnerResponse
    {
        public long ApprenticeshipId { get; set; }
        public long Ukprn { get; set; }
        public string ProviderName { get; set; }

        public DateTime LearnStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public int StandardCode { get; set; }
        public string StandardUId { get; set; }
        public string StandardReference { get; set; }
        public string StandardName { get; set; }
        public int? CompletionStatus { get; set; }
        public string Outcome { get; set; }
        public DateTime? ApprovalsStopDate { get; set; }
        public DateTime? ApprovalsPauseDate { get; set; }
        public DateTime? AchievementDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }

        public long Uln { get; set; }

        public string GivenNames { get; set; }
        public string FamilyName { get; set; }
    }
}
