using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Application.Models;
using SFA.DAS.EmployerAccounts.Configuration;
using SFA.DAS.EmployerAccounts.ExternalApi;
using SFA.DAS.EmployerAccounts.ExternalApi.Requests;
using SFA.DAS.EmployerAccounts.ExternalApi.Responses;
using SFA.DAS.EmployerAccounts.Helpers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
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
    private readonly ICompaniesHouseApiClient<CompaniesHouseApiConfiguration> _companiesHouseApi;

    public SearchOrganisationsQueryHandler(ILogger<SearchOrganisationsQueryHandler> logger,
        IReferenceDataApiClient<ReferenceDataApiConfiguration> referenceDataApiClient,
        IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> educationalOrganisationClient,
        IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration> publicSectotOrganisationApiClient,
        ICompaniesHouseApiClient<CompaniesHouseApiConfiguration> companiesHouseApiClient)
    {
        _logger = logger;
        _refDataApi = referenceDataApiClient;
        _eduOrgApi = educationalOrganisationClient;
        _psOrgApi = publicSectotOrganisationApiClient;
        _companiesHouseApi = companiesHouseApiClient;
    }

    public async Task<SearchOrganisationsResult> Handle(SearchOrganisationsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching for Organisation with searchTerm: {SearchTerm}", request.SearchTerm);

        var refApiOrganisationsTask = _refDataApi.Get<GetSearchOrganisationsResponse>(new GetSearchOrganisationsRequest(request.SearchTerm, request.MaximumResults));
        var educationalOrganisationsTask = _eduOrgApi.Get<EducationalOrganisationResponse>(new SearchEducationalOrganisationsRequest(request.SearchTerm, request.MaximumResults));
        //var publicSectorOrganisationsTask = _psOrgApi.Get<PublicSectorOrganisationsResponse>(new SearchPublicSectorOrganisationsRequest(request.SearchTerm));
        var companiesTask = GetCompaniesHouseSearchTask(request.SearchTerm, request.MaximumResults);

        await Task.WhenAll(refApiOrganisationsTask, educationalOrganisationsTask, companiesTask); //, publicSectorOrganisationsTask);
        var refApiOrganisations = await refApiOrganisationsTask;
        var educationalOrganisations = await educationalOrganisationsTask;
        //var publicSectorOrganisations = await publicSectorOrganisationsTask;
        var companies = await ProcessCompaniesSearchTask(companiesTask);

        var result = CombineMatches(
            refApiOrganisations.Where(o => o.Type != OrganisationType.EducationOrganisation && o.Type != OrganisationType.Company),
            educationalOrganisations.EducationalOrganisations,
            companies,
            request.MaximumResults);
        //publicSectorOrganisations.PublicSectorOrganisations, request.MaximumResults);

        return result;
    }

    private static SearchOrganisationsResult CombineMatches(
        IEnumerable<Organisation> refApiOrganisations,
        IEnumerable<EducationalOrganisation> educationalOrganisations,
        IEnumerable<OrganisationResult> companiesResults,
        /*IEnumerable<PublicSectorOrganisation> psOrganisations, */ int maxResults)
    {
        var allOrganisations = refApiOrganisations.Select(o => (OrganisationResult)o)
                .Concat(educationalOrganisations.Select(o => (OrganisationResult)o))
                .Concat(companiesResults);
        //.Concat(psOrganisations.Select(o=> (OrganisationResult)o));

        return new SearchOrganisationsResult
        {
            Organisations = allOrganisations.OrderBy(o => o.Name).Take(maxResults).ToList()
        };
    }

    private Task<object> GetCompaniesHouseSearchTask(string searchTerm, int maximumResults)
    {
        if (RegexHelper.CheckCompaniesHouseReference(searchTerm))
        {
            return _companiesHouseApi.Get<GetCompanyInfoResponse>(new GetCompanyInformationRequest(searchTerm)).ContinueWith(t => (object)t.Result);
        }
        else
        {
            return _companiesHouseApi.Get<SearchCompaniesResponse>(new SearchCompanyInformationRequest(searchTerm, maximumResults)).ContinueWith(t => (object)t.Result);
        }
    }

    private static async Task<IEnumerable<OrganisationResult>> ProcessCompaniesSearchTask(Task<object> companiesTask)
    {
        var result = await companiesTask;
        return result switch
        {
            GetCompanyInfoResponse singleCompany => new List<OrganisationResult> { singleCompany },
            SearchCompaniesResponse multipleCompanies => multipleCompanies?.Companies.Select(c => (OrganisationResult)c),
            _ => []
        };
    }
}
