using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Services;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.MigrateData
{
    public record MigrateDataQueryHandler : IRequestHandler<MigrateDataQuery, MigrateDataQueryResult>
    {
        private readonly ILegacyApplicationMigrationService _legacyApplicationMigrationService;

        public MigrateDataQueryHandler(ILegacyApplicationMigrationService legacyApplicationMigrationService)
        {
            _legacyApplicationMigrationService = legacyApplicationMigrationService;
        }

        public async Task<MigrateDataQueryResult> Handle(MigrateDataQuery request,
            CancellationToken cancellationToken)
        {
            var applications = await _legacyApplicationMigrationService.GetLegacyApplications(request.EmailAddress);

            return new MigrateDataQueryResult(applications);
        }
    }
}
