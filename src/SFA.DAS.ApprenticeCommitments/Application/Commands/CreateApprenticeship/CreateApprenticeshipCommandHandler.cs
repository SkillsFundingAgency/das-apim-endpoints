using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeCommitments.Application.Services;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipCommandHandler : IRequestHandler<CreateApprenticeshipCommand>
    {
        private readonly ApprenticeCommitmentsService _service;

        public CreateApprenticeshipCommandHandler(ApprenticeCommitmentsService service) => _service = service;

        public async Task<Unit> Handle(CreateApprenticeshipCommand command, CancellationToken cancellationToken)
        {
            await _service.CreateApprenticeship(Guid.NewGuid(), command.ApprenticeshipId, command.Email);
            return Unit.Value;
        }
    }
}