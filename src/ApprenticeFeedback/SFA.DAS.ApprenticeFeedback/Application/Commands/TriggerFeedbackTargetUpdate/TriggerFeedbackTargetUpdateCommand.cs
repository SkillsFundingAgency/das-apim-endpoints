using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.TriggerFeedbackTargetUpdate
{
    public class TriggerFeedbackTargetUpdateCommand : IRequest<TriggerFeedbackTargetUpdateResponse>
    {
        public long ApprenticeshipId { get; set; }
        public Guid ApprenticeFeedbackTargetId { get; set; }
    }
}
