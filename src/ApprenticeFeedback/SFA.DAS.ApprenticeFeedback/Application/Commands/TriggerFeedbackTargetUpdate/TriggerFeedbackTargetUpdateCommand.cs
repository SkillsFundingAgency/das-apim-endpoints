using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.TriggerFeedbackTargetUpdate
{
    public class TriggerFeedbackTargetUpdateCommand : IRequest<TriggerFeedbackTargetUpdateResponse>
    {
        public Guid Id { get; set; }
        public Guid ApprenticeId { get; set; }
    }
}
