using MediatR;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;
public class GetLiveVacanciesQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
    ILiveVacancyMapper liveVacancyMapper,
    ICourseService courseService)
    : IRequestHandler<GetLiveVacanciesQuery, GetLiveVacanciesQueryResult>
{
    public async Task<GetLiveVacanciesQueryResult> Handle(GetLiveVacanciesQuery request, CancellationToken cancellationToken)
    {
        var vacanciesResponseTask = recruitApiClient.GetWithResponseCode<GetLiveVacanciesApiResponse>(new GetLiveVacanciesApiRequest(request.PageNumber, request.PageSize, request.ClosingDate));
        var standardsTask = courseService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));

        await Task.WhenAll(vacanciesResponseTask, standardsTask);
        var vacanciesResponse = vacanciesResponseTask.Result;

        return new GetLiveVacanciesQueryResult
        {
            PageSize = vacanciesResponse.Body.PageSize,
            PageNo = vacanciesResponse.Body.PageNo,
            TotalLiveVacanciesReturned = vacanciesResponse.Body.TotalLiveVacanciesReturned,
            TotalLiveVacancies = vacanciesResponse.Body.TotalLiveVacancies,
            TotalPages = vacanciesResponse.Body.TotalPages,
            Vacancies = vacanciesResponse.Body.Vacancies.Select(x => liveVacancyMapper.Map(x, standardsTask.Result))
        };
    }
}
