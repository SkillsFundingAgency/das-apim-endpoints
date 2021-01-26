using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeCommitments.Application.Services;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipCommandHandler : IRequestHandler<CreateApprenticeshipCommand>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;
        private readonly ApprenticeLoginService _apprenticeLoginService;

        public CreateApprenticeshipCommandHandler(ApprenticeCommitmentsService apprenticeCommitmentsService, ApprenticeLoginService apprenticeLoginService)
        {
            _apprenticeCommitmentsService = apprenticeCommitmentsService;
            _apprenticeLoginService = apprenticeLoginService;
        }

        public async Task<Unit> Handle(CreateApprenticeshipCommand command, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            await _apprenticeCommitmentsService.CreateApprenticeship(id, command.ApprenticeshipId, command.Email);
            await _apprenticeLoginService.SendInvitation(id, command.Email);

            return Unit.Value;
        }
    }
}