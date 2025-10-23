using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class UpsertFeedbackTransactionRequest : IPostApiRequest<UpsertFeedbackTransactionData>
    {
        public string PostUrl => $"api/accounts/{AccountId}/feedbacktransaction";
        
        public long AccountId { get; }
        public UpsertFeedbackTransactionData Data { get; set; }

        public UpsertFeedbackTransactionRequest(long accountId, UpsertFeedbackTransactionData data)
        {
            AccountId = accountId;
            Data = data;
        }
    }
}