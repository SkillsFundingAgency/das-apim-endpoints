using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsQueryHandler : IRequestHandler<SearchOrganisationsQuery, SearchOrganisationsResult>
    {
        private readonly ILogger<SearchOrganisationsQueryHandler> _logger;
        private readonly IReferenceDataApiClient<ReferenceDataApiConfiguration> _refDataApi;
        private readonly IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> _eduOrgApi;


        public SearchOrganisationsQueryHandler(ILogger<SearchOrganisationsQueryHandler> logger,
            IReferenceDataApiClient<ReferenceDataApiConfiguration> referenceDataApiClient,
            IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> educationalOrganisationClient)
        {
            _logger = logger;
            _refDataApi = referenceDataApiClient;
            _eduOrgApi = educationalOrganisationClient;

        }

        public async Task<SearchOrganisationsResult> Handle(SearchOrganisationsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Searching for Organisation with searchTerm: {SearchTerm}", request.SearchTerm);

            var refApiOrganisationsTask = _refDataApi.Get<GetSearchOrganisationsResponse>(new GetSearchOrganisationsRequest(request.SearchTerm, request.MaximumResults));
            var educationalOrganisationsTask = _eduOrgApi.Get<EducationalOrganisationResponse>(new SearchEducationalOrganisationsRequest(request.SearchTerm, request.MaximumResults));

            await Task.WhenAll(refApiOrganisationsTask, educationalOrganisationsTask);
            var refApiOrganisations = await refApiOrganisationsTask;
            var educationalOrganisations = await educationalOrganisationsTask;

            var result = AggregateOrganisations(educationalOrganisations, refApiOrganisations);

            return result;
        }

        private SearchOrganisationsResult AggregateOrganisations(EducationalOrganisationResponse educationalOrganisations, GetSearchOrganisationsResponse refApiOrganisations)
        {
            var eduOrgCodes = educationalOrganisations.EducationalOrganisations.Select(org => org.URN).ToList();
            var filteredRefApiOrganisations = refApiOrganisations?.Where(org => !eduOrgCodes.Contains(org.Code)).ToList();

            SearchOrganisationsResult organisations = educationalOrganisations;

            organisations.Organisations.AddRange(filteredRefApiOrganisations.Select(x =>
                new Models.Organisation
                {
                    Name = x.Name,
                    Type = x.Type,
                    SubType = x.SubType,
                    Code = x.Code,
                    RegistrationDate = x.RegistrationDate,
                    Address = x.Address,
                    Sector = x.Sector,
                    OrganisationStatus = x.OrganisationStatus
                }).ToList()
            );

            return organisations;
        }
    }
}
