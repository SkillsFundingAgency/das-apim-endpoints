using MediatR;
using SFA.DAS.FindAnApprenticeship.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.MigrateLegacyApplications
{
    public record MigrateApplicationsCommandHandler(
        ILegacyApplicationMigrationService LegacyApplicationMigrationService)
        : IRequestHandler<MigrateApplicationsCommand, Unit>
    {
        public async Task<Unit> Handle(MigrateApplicationsCommand command, CancellationToken cancellationToken)
        {
            await LegacyApplicationMigrationService.MigrateLegacyApplications(command.CandidateId, command.EmailAddress);
            return Unit.Value;
        }
    }
}