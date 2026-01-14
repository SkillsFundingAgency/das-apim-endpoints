using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class GenerateFeedbackSummariesRequest : IPostApiRequest
    {
        public string PostUrl => "api/dataload/generate-feedback-summaries";
        public object Data { get; set; }
    }
}
