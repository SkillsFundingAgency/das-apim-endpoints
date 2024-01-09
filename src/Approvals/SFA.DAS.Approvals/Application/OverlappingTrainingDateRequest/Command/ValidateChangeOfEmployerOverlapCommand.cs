using MediatR;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Command
{
    public class ValidateChangeOfEmployerOverlapCommand : IRequest
    {
        public ValidateChangeOfEmployerOverlapRequest ValidateChangeOfEmployerOverlapRequest { get; set; }
    }
}
