using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Users.MigrateData
{
    public record MigrateDataQueryHandler : IRequestHandler<MigrateDataQuery, MigrateDataQueryResult>
    {
        private readonly ILegacyApplicationMigrationService _legacyApplicationMigrationService;
        private readonly IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration> _legacyApiClient;
        private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

        public MigrateDataQueryHandler(
            ILegacyApplicationMigrationService legacyApplicationMigrationService,
            IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration> legacyApiClient,
            ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        {
            _legacyApplicationMigrationService = legacyApplicationMigrationService;
            _legacyApiClient = legacyApiClient;
            _candidateApiClient = candidateApiClient;
        }

        public async Task<MigrateDataQueryResult> Handle(MigrateDataQuery request,
            CancellationToken cancellationToken)
        {
            var applications = _legacyApplicationMigrationService.GetLegacyApplications(request.EmailAddress);
            var existingUser = _candidateApiClient.GetWithResponseCode<GetCandidateApiResponse>(
                    new GetCandidateApiRequest(request.CandidateId.ToString()));
            var userDetails =
                _legacyApiClient.Get<GetLegacyUserByEmailApiResponse>(
                    new GetLegacyUserByEmailApiRequest(request.EmailAddress));

            await Task.WhenAll(applications, existingUser, userDetails);
            
            return new MigrateDataQueryResult(applications.Result, userDetails.Result, existingUser.Result.Body);
        }
    }
}
