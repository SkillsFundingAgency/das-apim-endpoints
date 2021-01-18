using MediatR;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Apprenticeship.Commands
{
    public class CreateApprenticeshipCommandHandler : IRequestHandler<CreateApprenticeshipCommand>
    {
        private readonly ApprenticeCommitmentsService service;

        public CreateApprenticeshipCommandHandler(ApprenticeCommitmentsService service) => this.service = service;

        public async Task<Unit> Handle(CreateApprenticeshipCommand command, CancellationToken cancellationToken)
        {
            await service.CreateApprenticeship(Guid.NewGuid(), command.ApprenticeshipId, command.Email);
            return Unit.Value;
        }
    }
}