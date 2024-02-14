using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsQueryHandler : IRequestHandler<SearchOrganisationsQuery, SearchOrganisationsResult>
    {
        private readonly ILogger<SearchOrganisationsQueryHandler> _logger;
        private readonly IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> _eduOrgApi;

        public SearchOrganisationsQueryHandler(ILogger<SearchOrganisationsQueryHandler> logger, IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> referenceDataApiClient)
        {
            _logger = logger;
            _eduOrgApi = referenceDataApiClient;
        }

        public async Task<SearchOrganisationsResult> Handle(SearchOrganisationsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Searching for Organisation with searchTerm: {request.SearchTerm}");

            var organisations = await _eduOrgApi.Get<EducationalOrganisationResponse>(new SearchEducationalOrganisationsRequest(request.SearchTerm, request.MaximumResults));

            return organisations;
        }
    }
}
