using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeCommitments.Application.Services;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.VerifyIdentityRegistration
{
    public class VerifyIdentityRegistrationCommandHandler : IRequestHandler<VerifyIdentityRegistrationCommand>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;

        public VerifyIdentityRegistrationCommandHandler(
            ApprenticeCommitmentsService apprenticeCommitmentsService)

        {
            _apprenticeCommitmentsService = apprenticeCommitmentsService;
        }

        public async Task<Unit> Handle(
            VerifyIdentityRegistrationCommand command,
            CancellationToken cancellationToken)
        {
            await _apprenticeCommitmentsService.VerifyRegistration(command);

            return Unit.Value;
        }
    }
}