using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class CreateExitSurveyRequest : IPostApiRequest
    {
        public string PostUrl => $"api/exitsurvey";

        public object Data { get; set; }


        public CreateExitSurveyRequest(CreateExitSurveyData data)
        {
            Data = data;
        }
    }

    public class CreateExitSurveyData
    {
        public Guid ApprenticeFeedbackTargetId { get; set; }
        public bool DidNotCompleteApprenticeship { get; set; }
        public string IncompletionReason { get; set; }
        // Incompletion factors:
        public bool IncompletionFactor_Caring { get; set; }
        public bool IncompletionFactor_Family { get; set; }
        public bool IncompletionFactor_Financial { get; set; }
        public bool IncompletionFactor_Mental { get; set; }
        public bool IncompletionFactor_Physical { get; set; }
        public bool IncompletionFactor_Other { get; set; }
        public string RemainedReason { get; set; }
        public string ReasonForIncorrect { get; set; }
        public bool AllowContact { get; set; }
    }
}
