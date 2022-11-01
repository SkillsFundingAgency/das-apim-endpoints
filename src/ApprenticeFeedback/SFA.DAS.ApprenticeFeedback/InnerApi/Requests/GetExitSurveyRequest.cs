using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GetExitSurveyRequest : IGetApiRequest
    {
        public string GetUrl => $"api/apprenticefeedbacktarget/{ApprenticeFeedbackTargetId}/exitsurvey";

        public Guid ApprenticeFeedbackTargetId { get; set; }

        public GetExitSurveyRequest(Guid apprenticeFeedbackTargetId)
        {
            ApprenticeFeedbackTargetId = apprenticeFeedbackTargetId;
        }
    }
}
