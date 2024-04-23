using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Application.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PublicSectorOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PublicSectorOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations;

public class SearchOrganisationsQueryHandler : IRequestHandler<SearchOrganisationsQuery, SearchOrganisationsResult>
{
    private readonly ILogger<SearchOrganisationsQueryHandler> _logger;
    private readonly IReferenceDataApiClient<ReferenceDataApiConfiguration> _refDataApi;
    private readonly IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> _eduOrgApi;
    private readonly IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration> _psOrgApi;


    public SearchOrganisationsQueryHandler(ILogger<SearchOrganisationsQueryHandler> logger,
        IReferenceDataApiClient<ReferenceDataApiConfiguration> referenceDataApiClient,
        IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> educationalOrganisationClient,
        IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration> publicSectotOrganisationApiClient)
    {
        _logger = logger;
        _refDataApi = referenceDataApiClient;
        _eduOrgApi = educationalOrganisationClient;
        _psOrgApi = publicSectotOrganisationApiClient;

    }

    public async Task<SearchOrganisationsResult> Handle(SearchOrganisationsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching for Organisation with searchTerm: {SearchTerm}", request.SearchTerm);

        var refApiOrganisationsTask = _refDataApi.Get<GetSearchOrganisationsResponse>(new GetSearchOrganisationsRequest(request.SearchTerm, request.MaximumResults));
        var educationalOrganisationsTask = _eduOrgApi.Get<EducationalOrganisationResponse>(new SearchEducationalOrganisationsRequest(request.SearchTerm, request.MaximumResults));
        var publicSectorOrganisationsTask = _psOrgApi.Get<PublicSectorOrganisationsResponse>(new SearchPublicSectorOrganisationsRequest(request.SearchTerm));

        await Task.WhenAll(refApiOrganisationsTask, educationalOrganisationsTask, publicSectorOrganisationsTask);
        var refApiOrganisations = await refApiOrganisationsTask;
        var educationalOrganisations = await educationalOrganisationsTask;

        var publicSectorOrganisations = await publicSectorOrganisationsTask;

        var result = CombineMatches(refApiOrganisations.Where(o=>o.Type != OrganisationType.PublicSector && o.Type != OrganisationType.EducationOrganisation),
            educationalOrganisations.EducationalOrganisations, 
            publicSectorOrganisations.PublicSectorOrganisations, request.MaximumResults);

        return result;
    }

    private static SearchOrganisationsResult CombineMatches(IEnumerable<Organisation> refApiOrganisations, IEnumerable<EducationalOrganisation> educationalOrganisations,
        IEnumerable<PublicSectorOrganisation> psOrganisations, int maxResults)
    {
        var allOrganisations = refApiOrganisations.Select(o=> (OrganisationResult)o)
                .Concat(educationalOrganisations.Select(o=> (OrganisationResult)o))
                .Concat(psOrganisations.Select(o=> (OrganisationResult)o));

        return new SearchOrganisationsResult
        {
            Organisations = allOrganisations.OrderBy(o => o.Name).Take(maxResults).ToList()
        };
    }
}