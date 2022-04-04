using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprenticeFeedbackTarget
{
    public class CreateApprenticeFeedbackTargetCommand : IRequest<CreateApprenticeFeedbackTargetResponse>
    {
        public Guid ApprenticeId { get; set; }
        public long ApprenticeshipId { get; set; }
        public long ConfirmationId { get; set; }
        public DateTime ConfirmedOn { get; set; }
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime CommitmentsApprovedOn { get; set; }
    }
}
