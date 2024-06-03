using MediatR;
using SFA.DAS.FindAnApprenticeship.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetLegacyApplications
{
    public record GetLegacyApplicationsQueryHandler : IRequestHandler<GetLegacyApplicationsQuery, GetLegacyApplicationsQueryResult>
    {
        private readonly ILegacyApplicationMigrationService _legacyApplicationMigrationService;

        public GetLegacyApplicationsQueryHandler(ILegacyApplicationMigrationService legacyApplicationMigrationService)
        {
            _legacyApplicationMigrationService = legacyApplicationMigrationService;
        }

        public async Task<GetLegacyApplicationsQueryResult> Handle(GetLegacyApplicationsQuery request,
            CancellationToken cancellationToken)
        {
            var applications = await _legacyApplicationMigrationService.GetLegacyApplications(request.EmailAddress);

            return new GetLegacyApplicationsQueryResult(applications);
        }
    }
}
