using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsQueryHandler : IRequestHandler<SearchOrganisationsQuery, SearchOrganisationsResult>
    {
        private readonly ILogger<SearchOrganisationsQueryHandler> _logger;
        private readonly IReferenceDataApiClient<ReferenceDataApiConfiguration> _refDataApi;

        public SearchOrganisationsQueryHandler(ILogger<SearchOrganisationsQueryHandler> logger, IReferenceDataApiClient<ReferenceDataApiConfiguration> referenceDataApiClient)
        {
            _logger = logger;
            _refDataApi = referenceDataApiClient;
        }

        public async Task<SearchOrganisationsResult> Handle(SearchOrganisationsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Searching for Organisation with searchTerm: {request.SearchTerm}");

            var organisations = await _refDataApi.Get<SearchOrganisationsResponse>(new SearchOrganisationsRequest(request.SearchTerm, request.MaximumResults));

            return organisations;
        }
    }
}
