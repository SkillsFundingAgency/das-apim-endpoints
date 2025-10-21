using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class UpdateFeedbackTransactionRequest : IPutApiRequest<UpdateFeedbackTransactionData>
    {
        public UpdateFeedbackTransactionRequest(long feedbackTransactionId, UpdateFeedbackTransactionData data)
        {
            FeedbackTransactionId = feedbackTransactionId;
            Data = data;
        }

        public long FeedbackTransactionId { get; }
        public UpdateFeedbackTransactionData Data { get; set; }

        public string PutUrl => $"api/feedbacktransactions/{FeedbackTransactionId}";
    }

    public class UpdateFeedbackTransactionData
    {
        public Guid TemplateId { get; set; }
        public int SentCount { get; set; }
        public DateTime SentDate { get; set; }
    }
}