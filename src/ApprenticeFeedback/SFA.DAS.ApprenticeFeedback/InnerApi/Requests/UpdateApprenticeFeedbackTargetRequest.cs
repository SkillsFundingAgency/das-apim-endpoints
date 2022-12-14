using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class UpdateApprenticeFeedbackTargetRequest : IPostApiRequest
    {
        public string PostUrl => "api/apprenticefeedbacktarget/update";

        public object Data { get; set; }

        public UpdateApprenticeFeedbackTargetRequest(UpdateApprenticeFeedbackTargetRequestData data)
        {
            Data = data;
        }
    }

    public class UpdateApprenticeFeedbackTargetRequestData
    {
        public Guid ApprenticeFeedbackTargetId { get; internal set; }
        public LearnerData Learner { get; set; }
    }

    public class LearnerData
    {
        public long Ukprn { get; set; }
        public string ProviderName { get; set; }
        public DateTime LearnStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public int StandardCode { get; set; }
        public string StandardUId { get; set; }
        public string StandardReference { get; set; }
        public string StandardName { get; set; }
        public int? CompletionStatus { get; set; }
        public DateTime? ApprovalsStopDate { get; set; }
        public DateTime? ApprovalsPauseDate { get; set; }
        public DateTime? LearnActEndDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }
        public bool IsTransfer { get; set; }
        public DateTime? DateTransferIdentified { get; set; }

        public static implicit operator LearnerData(GetApprenticeLearnerResponse source)
        {
            if(source == null)
            {
                return null;
            }

            return new LearnerData
            {
                Ukprn = source.Ukprn,
                ProviderName = source.ProviderName,
                LearnActEndDate = source.LearnActEndDate,
                EstimatedEndDate = source.EstimatedEndDate,
                ApprovalsStopDate = source.ApprovalsStopDate,
                ApprovalsPauseDate = source.ApprovalsPauseDate,
                CompletionStatus = source.CompletionStatus,
                LearnStartDate = source.LearnStartDate,
                PlannedEndDate = source.PlannedEndDate,
                StandardCode = source.StandardCode,
                StandardName = source.StandardName,
                StandardReference = source.StandardReference,
                StandardUId = source.StandardUId,
                IsTransfer = source.IsTransfer,
                DateTransferIdentified = source.DateTransferIdentified
            };
        }
    }
}
