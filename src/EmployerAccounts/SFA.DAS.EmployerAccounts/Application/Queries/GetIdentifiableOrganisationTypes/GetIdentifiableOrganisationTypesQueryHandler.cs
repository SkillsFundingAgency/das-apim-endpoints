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
        private readonly IReferenceDataApiClient<ReferenceDataApiConfiguration> _refDataApi;

        public GetIdentifiableOrganisationTypesQueryHandler(ILogger<GetIdentifiableOrganisationTypesQueryHandler> logger, IReferenceDataApiClient<ReferenceDataApiConfiguration> referenceDataApiClient)
        {
            _logger = logger;
            _refDataApi = referenceDataApiClient;
        }

        public async Task<GetIdentifiableOrganisationTypesResult> Handle(GetIdentifiableOrganisationTypesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Getting GetIdentifiableOrganisationTypes from reference api");

            var organisationTypes = await _refDataApi.Get<OrganisationType[]>(new IdentifiableOrganisationTypesRequest());

            return new GetIdentifiableOrganisationTypesResult
            {
                OrganisationTypes = organisationTypes
            };
        }
    }
}
