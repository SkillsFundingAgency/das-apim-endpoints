using MediatR;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeEmailAddress
{
    public class ChangeEmailAddressCommand : IRequest
    {
        public ChangeEmailAddressCommand(long apprenticeId, string email) =>
            (ApprenticeId, Email) = (apprenticeId, email);

        public long ApprenticeId { get; }
        public string Email { get; }
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