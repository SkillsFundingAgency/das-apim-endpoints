using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class UpdateApprenticeFeedbackTargetDeferRequest : IPostApiRequest<UpdateApprenticeFeedbackTargetDeferRequestData>
    {
        public string PostUrl => "api/apprenticefeedbacktarget/deferUpdate";

        public UpdateApprenticeFeedbackTargetDeferRequestData Data { get; set; }

        public UpdateApprenticeFeedbackTargetDeferRequest(UpdateApprenticeFeedbackTargetDeferRequestData data)
        {
            Data = data;
        }
    }

    public class UpdateApprenticeFeedbackTargetDeferRequestData
    {
        public Guid ApprenticeFeedbackTargetId { get; internal set; }
    }
}
