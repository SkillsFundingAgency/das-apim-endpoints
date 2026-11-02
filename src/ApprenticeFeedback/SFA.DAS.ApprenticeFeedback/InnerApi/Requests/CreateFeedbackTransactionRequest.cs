using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class CreateFeedbackTransactionRequest : IPostApiRequest
    {
        public string PostUrl => "api/feedbacktransaction";

        public object Data { get; set; }

        public CreateFeedbackTransactionRequest(CreateFeedbackTransactionData data)
        {
            Data = data;
        }
    }

    public class CreateFeedbackTransactionData
    {
        public long ApprenticeshipId { get; set; }
        public string TemplateName { get; set; }

        public CreateFeedbackTransactionData(long apprenticeshipId, string templateName)
        {
            ApprenticeshipId = apprenticeshipId;
            TemplateName = templateName;
        }
    }
}
