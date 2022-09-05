using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class UpdateApprenticeFeedbackTargetStatusRequest : IPatchApiRequest<UpdateApprenticeFeedbackTargetStatusRequestData>
    {
        public string PatchUrl => $"api/ApprenticeFeedbackTarget/{Data.ApprenticeFeedbackTargetId}";

        public UpdateApprenticeFeedbackTargetStatusRequestData Data { get; set; }

        public UpdateApprenticeFeedbackTargetStatusRequest(UpdateApprenticeFeedbackTargetStatusRequestData data)
        {
            Data = data;
        }
    }

    public class UpdateApprenticeFeedbackTargetStatusRequestData
    {
        public Guid ApprenticeFeedbackTargetId { get; internal set; }
        public int Status { get; set; }
        public int FeedbackEligibilityStatus { get; set; }
    }
}
