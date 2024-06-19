using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Application.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Charities;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Charities;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;
namespace SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations;

public class SearchOrganisationsQueryHandler : IRequestHandler<SearchOrganisationsQuery, SearchOrganisationsResult>
{
    private readonly ILogger<SearchOrganisationsQueryHandler> _logger;
    private readonly IReferenceDataApiClient<ReferenceDataApiConfiguration> _refDataApi;
    private readonly IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> _eduOrgApi;
    private readonly IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration> _psOrgApi;
    private readonly ICharitiesApiClient<CharitiesApiConfiguration> _charitiesApi;


    public SearchOrganisationsQueryHandler(ILogger<SearchOrganisationsQueryHandler> logger,
        IReferenceDataApiClient<ReferenceDataApiConfiguration> referenceDataApiClient,
        IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> educationalOrganisationClient,
        IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration> publicSectotOrganisationApiClient,
        ICharitiesApiClient<CharitiesApiConfiguration> charitiesApi)
    {
        _logger = logger;
        _refDataApi = referenceDataApiClient;
        _eduOrgApi = educationalOrganisationClient;
        _psOrgApi = publicSectotOrganisationApiClient;
        _charitiesApi = charitiesApi;
    }

    public async Task<SearchOrganisationsResult> Handle(SearchOrganisationsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching for Organisation with searchTerm: {SearchTerm}", request.SearchTerm);

        var refApiOrganisationsTask = _refDataApi.Get<GetSearchOrganisationsResponse>(new GetSearchOrganisationsRequest(request.SearchTerm, request.MaximumResults));
        var educationalOrganisationsTask = _eduOrgApi.Get<EducationalOrganisationResponse>(new SearchEducationalOrganisationsRequest(request.SearchTerm, request.MaximumResults));
        //var publicSectorOrganisationsTask = _psOrgApi.Get<PublicSectorOrganisationsResponse>(new SearchPublicSectorOrganisationsRequest(request.SearchTerm));

        Task charitiesSearchTask;

        if (int.TryParse(request.SearchTerm, out int registrationNumber))
        {
            charitiesSearchTask = _charitiesApi.Get<GetCharityResponse>(new GetCharityByRegistrationNumberRequest(registrationNumber));
        }
        else
        {
            charitiesSearchTask = _charitiesApi.Get<SearchCharitiesResponse>(new SearchCharitiesRequest(request.SearchTerm, request.MaximumResults));
        }

        await Task.WhenAll(refApiOrganisationsTask, educationalOrganisationsTask, charitiesSearchTask); //, publicSectorOrganisationsTask);
        var refApiOrganisations = await refApiOrganisationsTask;
        var educationalOrganisations = await educationalOrganisationsTask;

        IEnumerable<OrganisationResult> charityResults;
        if (charitiesSearchTask is Task<GetCharityResponse> singleCharitySearchTask)
        {
            var charityResponse = await singleCharitySearchTask;
            charityResults = [(OrganisationResult)charityResponse];
        }
        else
        {
            var charityResponse = await (Task<SearchCharitiesResponse>)charitiesSearchTask;
            charityResults = charityResponse != null ? charityResponse?.Select(c => (OrganisationResult)c) : [];
        }

        //var publicSectorOrganisations = await publicSectorOrganisationsTask;

        var result = CombineMatches(
            refApiOrganisations.Where(o => o.Type != OrganisationType.EducationOrganisation && o.Type != OrganisationType.Charity),
            educationalOrganisations?.EducationalOrganisations,
            charityResults,
            request.MaximumResults);
        //publicSectorOrganisations.PublicSectorOrganisations, request.MaximumResults);

        return result;
    }

    private static SearchOrganisationsResult CombineMatches(IEnumerable<Organisation> refApiOrganisations,
        IEnumerable<EducationalOrganisation> educationalOrganisations,
        IEnumerable<OrganisationResult> charityResults,
         /*IEnumerable<PublicSectorOrganisation> psOrganisations, */
         int maxResults)
    {
        var allOrganisations = refApiOrganisations.Select(o => (OrganisationResult)o)
                .Concat(educationalOrganisations.Select(o => (OrganisationResult)o))
                .Concat(charityResults);

        //.Concat(psOrganisations.Select(o=> (OrganisationResult)o));

        return new SearchOrganisationsResult
        {
            Organisations = allOrganisations.OrderBy(o => o.Name).Take(maxResults).ToList()
        };
    }
}
