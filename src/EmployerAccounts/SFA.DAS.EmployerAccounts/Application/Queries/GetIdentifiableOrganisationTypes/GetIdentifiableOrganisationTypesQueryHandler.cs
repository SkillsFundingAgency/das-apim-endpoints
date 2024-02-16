using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetIdentifiableOrganisationTypes
{
    public class GetIdentifiableOrganisationTypesQueryHandler : IRequestHandler<GetIdentifiableOrganisationTypesQuery, GetIdentifiableOrganisationTypesResult>
    {
        private readonly ILogger<GetIdentifiableOrganisationTypesQueryHandler> _logger;
        private readonly IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> _eduOrgApi;

        public GetIdentifiableOrganisationTypesQueryHandler(ILogger<GetIdentifiableOrganisationTypesQueryHandler> logger,
             IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> educationalOrganisationApiClient)
        {
            _logger = logger;
            _eduOrgApi = educationalOrganisationApiClient;
        }

        public async Task<GetIdentifiableOrganisationTypesResult> Handle(GetIdentifiableOrganisationTypesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Getting GetIdentifiableOrganisationTypes from educational-organisations api");

            var organisationTypes = await _eduOrgApi.Get<string[]>(new IdentifiableOrganisationTypesRequest());

            return new GetIdentifiableOrganisationTypesResult
            {
                OrganisationTypes = organisationTypes
            };
        }
    }
}
