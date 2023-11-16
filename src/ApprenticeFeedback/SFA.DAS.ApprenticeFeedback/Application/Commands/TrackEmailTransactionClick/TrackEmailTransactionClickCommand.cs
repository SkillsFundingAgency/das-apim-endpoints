using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.TrackEmailTransactionClick
{
    public class TrackEmailTransactionClickCommand : IRequest<TrackEmailTransactionClickResponse>
    {
        public long FeedbackTransactionId { get; set; }
        public Guid ApprenticeFeedbackTargetId { get; set; }
        public string LinkName { get; set; }
        public string LinkUrl { get; set; }
        public DateTime ClickedOn { get; set; }
    }
}
