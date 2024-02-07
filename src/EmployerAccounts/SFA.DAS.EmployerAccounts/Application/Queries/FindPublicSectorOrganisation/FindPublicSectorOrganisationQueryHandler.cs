using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerAccounts.Application.Queries.FindPublicSectorOrganisation
{
    public class FindPublicSectorOrganisationQueryHandler : IRequestHandler<FindPublicSectorOrganisationQuery, PagedResponse<FindPublicSectorOrganisationResult>>
    {
        private readonly ILogger<FindPublicSectorOrganisationQueryHandler> _logger;
        private readonly IReferenceDataApiClient<ReferenceDataApiConfiguration> _refDataApi;

        public FindPublicSectorOrganisationQueryHandler(ILogger<FindPublicSectorOrganisationQueryHandler> logger, IReferenceDataApiClient<ReferenceDataApiConfiguration> referenceDataApiClient)
        {
            _logger = logger;
            _refDataApi = referenceDataApiClient;
        }

        public async Task<PagedResponse<FindPublicSectorOrganisationResult>> Handle(FindPublicSectorOrganisationQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Searching for PublicSectorOrganisations with searchTerm: {request.SearchTerm}");

            var apiResponse = await _refDataApi.GetPaged<GetPublicSectorOrganisationsResponse>(new GetPublicSectorOrganisationsRequest(request.SearchTerm, request.PageSize, request.PageNumber));

            return new PagedResponse<FindPublicSectorOrganisationResult>
            {
                Data = apiResponse.Data.Select(x => (FindPublicSectorOrganisationResult)x).ToList(),
                Page = apiResponse.Page,
                TotalPages = apiResponse.TotalPages
            };
        }
    }
}
