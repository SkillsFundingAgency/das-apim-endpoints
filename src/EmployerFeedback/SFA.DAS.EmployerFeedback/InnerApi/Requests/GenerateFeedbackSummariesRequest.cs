using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class GenerateFeedbackSummariesRequest : IPostApiRequest
    {
        public string PostUrl => "api/dataload/generate-feedback-summaries";
        public object Data { get; set; }
    }
}
