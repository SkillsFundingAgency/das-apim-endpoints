using MediatR;
using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateFeedbackTarget
{
    public class CreateFeedbackTargetCommand : IRequest<CreateFeedbackTargetResponse>
    {
        public Guid ApprenticeId { get; set; }
        public long ApprenticeshipId { get; set; }
        public long ConfirmationId { get; set; }
        public DateTime ConfirmedOn { get; set; }
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime CommitmentsApprovedOn { get; set; }
    }
}
