using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GenerateEmailTransactionRequest : IPostApiRequest
    {
        public string PostUrl => "api/feedbacktransaction";

        public object Data { get; set; }

        public GenerateEmailTransactionRequest(GenerateEmailTransactionData data)
        {
            Data = data;
        }
    }

    public class GenerateEmailTransactionData
    {
    }
}
