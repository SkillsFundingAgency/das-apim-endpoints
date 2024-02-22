using MediatR;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Command
{
    public class ValidateDraftApprenticeshipDetailsCommand : IRequest
    {
        public ValidateDraftApprenticeshipRequest DraftApprenticeshipRequest { get; set; }

    }
}
