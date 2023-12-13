using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.TriggerFeedbackTargetUpdate
{
    public class TriggerFeedbackTargetUpdateCommand : IRequest<TriggerFeedbackTargetUpdateResponse>
    {
        public Guid ApprenticeFeedbackTargetId { get; set; }
        public Guid ApprenticeId { get; set; }
        public long ApprenticeshipId { get; set; }
        
    }
}
