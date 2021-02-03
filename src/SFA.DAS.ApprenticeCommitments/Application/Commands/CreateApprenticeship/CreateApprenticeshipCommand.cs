using MediatR;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipCommand : IRequest
    {
        public long EmployerAccountId { get; set; }
        public long ApprenticeshipId { get; set; }
        public string Email { get; set; }
        public string Organisation { get; set; }
    }
}