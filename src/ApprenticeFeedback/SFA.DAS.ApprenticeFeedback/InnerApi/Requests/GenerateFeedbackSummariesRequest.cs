using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GenerateFeedbackSummariesRequest : IPostApiRequest
    {
        public string PostUrl => "api/dataload/generate-feedback-summaries";
        public object Data { get; set; }
    }
}
