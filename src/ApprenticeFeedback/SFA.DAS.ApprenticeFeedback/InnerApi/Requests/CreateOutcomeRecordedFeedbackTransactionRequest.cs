using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class CreateOutcomeRecordedFeedbackTransactionRequest : IPostApiRequest
    {
        public string PostUrl => "api/feedbacktransaction/outcome-recorded";

        public object Data { get; set; }

        public CreateOutcomeRecordedFeedbackTransactionRequest(CreateOutcomeRecordedFeedbackTransactionData data)
        {
            Data = data;
        }
    }

    public class CreateOutcomeRecordedFeedbackTransactionData
    {
        public long ApprenticeshipId { get; set; }

        public CreateOutcomeRecordedFeedbackTransactionData(long apprenticeshipId)
        {
            ApprenticeshipId = apprenticeshipId;
        }
    }
}
