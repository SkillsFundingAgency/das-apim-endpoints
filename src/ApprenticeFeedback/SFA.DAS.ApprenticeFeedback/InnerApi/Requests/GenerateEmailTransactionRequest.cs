using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GenerateEmailTransactionRequest : IPostApiRequest
    {
        public string PostUrl => "api/feedbacktransaction/generate-email-transactions";

        public object Data { get; set; }
    }
}
