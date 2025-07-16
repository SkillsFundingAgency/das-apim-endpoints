using MediatR;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.CivilServiceJobs;
public class GetCivilServiceJobsQueryHandler(
    ICivilServiceJobsApiClient apiClient,
    ILiveVacancyMapper liveVacancyMapper,
    ILocationApiClient<LocationApiConfiguration> locationApiClient,
    ICourseService courseService) : IRequestHandler<GetCivilServiceJobsQuery, GetCivilServiceJobsQueryResult>
{
    public async Task<GetCivilServiceJobsQueryResult> Handle(GetCivilServiceJobsQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.GetWithResponseCode(new GetCivilServiceJobsApiRequest());

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return new GetCivilServiceJobsQueryResult
            {
                CivilServiceVacancies = []
            };
        }
        
        var civilServiceVacancies = response.Body.ToList();
        if (!civilServiceVacancies.Any())
        {
            return new GetCivilServiceJobsQueryResult
            {
                CivilServiceVacancies = []
            };
        }

        return new GetCivilServiceJobsQueryResult();
    }
}
