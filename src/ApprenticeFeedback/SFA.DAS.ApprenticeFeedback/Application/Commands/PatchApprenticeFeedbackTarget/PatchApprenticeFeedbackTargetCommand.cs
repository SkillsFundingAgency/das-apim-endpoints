using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.PatchApprenticeFeedbackTarget
{
    public class PatchApprenticeFeedbackTargetCommand : IRequest<PatchApprenticeFeedbackTargetResponse>
    {
        public Guid ApprenticeFeedbackTargetId { get; set; }
        public int Status { get; set; }

    }
}
