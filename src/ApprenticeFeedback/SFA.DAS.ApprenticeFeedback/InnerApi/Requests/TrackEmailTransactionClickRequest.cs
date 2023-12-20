using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class TrackEmailTransactionClickRequest : IPostApiRequest
    {
        public string PostUrl => $"api/feedbacktransaction/{((TrackEmailTransactionClickData)Data).FeedbackTransactionId}/track-click";

        public object Data { get; set; }

        public TrackEmailTransactionClickRequest(TrackEmailTransactionClickData data)
        {
            Data = data;
        }
    }

    public class TrackEmailTransactionClickData
    {
        public long FeedbackTransactionId { get; set; }
        public Guid ApprenticeFeedbackTargetId { get; set; }
        public string LinkName { get; set; }
        public string LinkUrl { get; set; }
        public DateTime ClickedOn { get; set; }

        public TrackEmailTransactionClickData(long feedbackTransactionId, Guid apprenticeFeedbackTargetId, string linkName, string linkUrl, DateTime clickedOn)
        {
            FeedbackTransactionId = feedbackTransactionId;
            ApprenticeFeedbackTargetId = apprenticeFeedbackTargetId;
            LinkName = linkName;
            LinkUrl = linkUrl;
            ClickedOn = clickedOn;
        }
    }
}
