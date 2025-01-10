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
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Charities;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PublicSectorOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Charities;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PublicSectorOrganisation;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations;

public class SearchOrganisationsQueryHandler(
    ILogger<SearchOrganisationsQueryHandler> logger,
    IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> educationalOrganisationClient,
    IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration> publicSectotOrganisationApiClient,
    ICompaniesHouseApiClient<CompaniesHouseApiConfiguration> companiesHouseApiClient,
    ICharitiesApiClient<CharitiesApiConfiguration> charitiesApi)
    : IRequestHandler<SearchOrganisationsQuery, SearchOrganisationsResult>
{
    public async Task<SearchOrganisationsResult> Handle(SearchOrganisationsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Searching for Organisation with searchTerm: {SearchTerm}", request.SearchTerm);
        
        var educationalOrganisationsTask = educationalOrganisationClient.Get<EducationalOrganisationResponse>(new SearchEducationalOrganisationsRequest(request.SearchTerm, request.MaximumResults));
        var publicSectorOrganisationsTask = publicSectotOrganisationApiClient.Get<PublicSectorOrganisationsResponse>(new SearchPublicSectorOrganisationsRequest(request.SearchTerm));
        var charitiesSearchTask = GetCharitiesSearchTask(request.SearchTerm, request.MaximumResults);
        var companiesTask = GetCompaniesHouseSearchTask(request.SearchTerm, request.MaximumResults);

        await Task.WhenAll(educationalOrganisationsTask, companiesTask, publicSectorOrganisationsTask, charitiesSearchTask);
        var educationalOrganisations = await educationalOrganisationsTask;
        var charityResults = await ProcessCharitiesSearchTask(charitiesSearchTask);
        var publicSectorOrganisations = await publicSectorOrganisationsTask;
        var companies = await ProcessCompaniesSearchTask(companiesTask);

        var result = CombineMatches(
            educationalOrganisations?.EducationalOrganisations,
            companies,
            charityResults,
            publicSectorOrganisations.PublicSectorOrganisations,
            request.MaximumResults);

        return result;
    }

    private static SearchOrganisationsResult CombineMatches(
    IEnumerable<EducationalOrganisation> educationalOrganisations,
    IEnumerable<OrganisationResult> companiesResults,
    IEnumerable<OrganisationResult> charityResults,
    IEnumerable<PublicSectorOrganisation> psOrganisations,
    int maxResults)
{
    var allOrganisations = educationalOrganisations
        .Select(o => (OrganisationResult)o)
        .Concat(psOrganisations.Select(o => (OrganisationResult)o))
        .Concat(charityResults)
        .Concat(companiesResults);
    

    return new SearchOrganisationsResult
    {
        Organisations = allOrganisations.OrderBy(o => o.Name).Take(maxResults).ToList()
    };
}


    private Task<object> GetCharitiesSearchTask(string searchTerm, int maximumResults)
    {
        if (int.TryParse(searchTerm, out int registrationNumber))
        {
            return charitiesApi.Get<GetCharityResponse>(new GetCharityByRegistrationNumberRequest(registrationNumber)).ContinueWith(t => (object)t.Result);
        }
        else
        {
            return charitiesApi.Get<SearchCharitiesResponse>(new SearchCharitiesRequest(searchTerm, maximumResults)).ContinueWith(t => (object)t.Result);
        }
    }

    private static async Task<IEnumerable<OrganisationResult>> ProcessCharitiesSearchTask(Task<object> charitiesSearchTask)
    {
        var result = await charitiesSearchTask;
        return result switch
        {
            GetCharityResponse singleCharity => new List<OrganisationResult> { singleCharity },
            SearchCharitiesResponse multipleCharities => multipleCharities.Select(c => (OrganisationResult)c),
            _ => []
        };
    }

    private Task<object> GetCompaniesHouseSearchTask(string searchTerm, int maximumResults)
    {
        if (RegexHelper.CheckCompaniesHouseReference(searchTerm))
        {
            return companiesHouseApiClient.Get<GetCompanyInfoResponse>(new GetCompanyInformationRequest(searchTerm)).ContinueWith(t => (object)t.Result);
        }
        else
        {
            return companiesHouseApiClient.Get<SearchCompaniesResponse>(new SearchCompanyInformationRequest(searchTerm, maximumResults)).ContinueWith(t => (object)t.Result);
        }
    }

    private static async Task<IEnumerable<OrganisationResult>> ProcessCompaniesSearchTask(Task<object> companiesTask)
    {
        var result = await companiesTask;
        return result switch
        {
            GetCompanyInfoResponse singleCompany => new List<OrganisationResult> { singleCompany },
            SearchCompaniesResponse multipleCompanies => multipleCompanies.Companies.Select(c => (OrganisationResult)c),
            _ => []
        };
    }
}