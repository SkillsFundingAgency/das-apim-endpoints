using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.UpdateApprenticeFeedbackTarget
{
    public class UpdateApprenticeFeedbackTargetCommand : IRequest<UpdateApprenticeFeedbackTargetResponse>
    {
        public Guid ApprenticeId { get; set; }
    }
}
