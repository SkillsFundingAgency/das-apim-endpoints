using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsQueryHandler : IRequestHandler<SearchOrganisationsQuery, SearchOrganisationsResult>
    {
        private readonly ILogger<SearchOrganisationsQueryHandler> _logger;
        private readonly IReferenceDataApiClient _apiClient;
        private readonly IReferenceDataApiClient<ReferenceDataApiConfiguration> _refDataApi;

        public SearchOrganisationsQueryHandler(ILogger<SearchOrganisationsQueryHandler> logger, IReferenceDataApiClient<ReferenceDataApiConfiguration> referenceDataApiClient, IReferenceDataApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
            _refDataApi = referenceDataApiClient;
        }

        public async Task<SearchOrganisationsResult> Handle(SearchOrganisationsQuery request, CancellationToken cancellationToken)
        {
            //version 0 = as it was
            //version 1 = restease
            // v 3 = as it was with version header.
            _logger.LogInformation("Searching for Organisation with searchTerm: {SearchTerm}", request.SearchTerm);

            if (request.Version == 0)
            {
                var organisations = await _refDataApi.Get<GetSearchOrganisationsResponse>(new GetSearchOrganisationsRequest(request.SearchTerm, request.MaximumResults));

                return new SearchOrganisationsResult(organisations);
            }
            
            var result = await _apiClient.SearchOrganisations(request.SearchTerm, request.MaximumResults, cancellationToken);

            if (result.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var response = result.GetContent();
                return new SearchOrganisationsResult(response);
            }

            _logger.LogInformation("Call to ReferenceData Api was not successful. There has been status returned of StatusCode {StatusCode}", result.ResponseMessage.StatusCode);

            return new SearchOrganisationsResult([]);
            
                    
        }
    }
}
