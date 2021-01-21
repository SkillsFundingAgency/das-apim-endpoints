using MediatR;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipCommand : IRequest
    {
        public long ApprenticeshipId { get; set; }
        public string Email { get; set; }
    }
}