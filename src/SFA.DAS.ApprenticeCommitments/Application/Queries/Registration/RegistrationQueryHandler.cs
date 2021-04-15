using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeCommitments.Application.Services;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.Registration
{
    public class RegistrationQueryHandler : IRequestHandler<RegistrationQuery, RegistrationResponse>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;

        public RegistrationQueryHandler(ApprenticeCommitmentsService apprenticeCommitmentsService)
        {
            _apprenticeCommitmentsService = apprenticeCommitmentsService;
        }

        public Task<RegistrationResponse> Handle(
            RegistrationQuery command,
            CancellationToken cancellationToken)
        {
            return _apprenticeCommitmentsService.GetRegistration(command.ApprenticeshipId);
        }
    }
}