using MediatR;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeEmailAddress
{
    public class ChangeEmailAddressCommand : IRequest
    {
        public long ApprenticeId { get; set; }
        public string Email { get; set; }
    }

    public class ChangeEmailAddressCommandHandler
        : IRequestHandler<ChangeEmailAddressCommand>
    {
        private readonly ApprenticeCommitmentsService _apprenticeCommitmentsService;

        public ChangeEmailAddressCommandHandler(
            ApprenticeCommitmentsService apprenticeCommitmentsService)
            => _apprenticeCommitmentsService = apprenticeCommitmentsService;

        public async Task<Unit> Handle(
            ChangeEmailAddressCommand command,
            CancellationToken cancellationToken)
        {
            await _apprenticeCommitmentsService.ChangeEmailAddress(
                command.ApprenticeId,
                command.Email);
            return Unit.Value;
        }
    }
}