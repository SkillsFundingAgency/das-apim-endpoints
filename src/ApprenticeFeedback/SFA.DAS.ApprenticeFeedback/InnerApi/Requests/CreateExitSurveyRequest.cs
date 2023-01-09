using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

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
        public bool AllowContact { get; set; }
        public List<int> AttributeIds { get; set; }
        public int PrimaryReason { get; set; }
    }
}
