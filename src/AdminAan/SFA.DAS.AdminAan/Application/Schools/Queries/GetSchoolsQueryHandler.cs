using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminAan.Application.Schools.Queries;
public class GetSchoolsQueryHandler(IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration> apiClient, ILogger<GetSchoolsQueryHandler> logger)
    : IRequestHandler<GetSchoolsQuery, GetSchoolsQueryApiResult>
{
    private const int PageSize = 100;

    public async Task<GetSchoolsQueryApiResult> Handle(GetSchoolsQuery request, CancellationToken cancellationToken)
    {
        var result = await apiClient.GetWithResponseCode<EducationalOrganisationResponse>(new SearchEducationalOrganisationsRequest(request.SearchTerm, PageSize));

        if (result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var response = result.Body;
            var educationalOrganisations = response.EducationalOrganisations;
            var schools = educationalOrganisations
                .Where(eduOrg => eduOrg.URN.Length == 6)
                .Select(eduOrg => new School
                {
                    Name = eduOrg.Name,
                    Urn = eduOrg.URN
                }).ToList();
            
            return new GetSchoolsQueryApiResult(schools);
        }

        logger.LogInformation("Call to ReferenceData Api was not successful. There has been status returned of StatusCode {StatusCode}", result.StatusCode);

        return new GetSchoolsQueryApiResult([]);
    }
}
