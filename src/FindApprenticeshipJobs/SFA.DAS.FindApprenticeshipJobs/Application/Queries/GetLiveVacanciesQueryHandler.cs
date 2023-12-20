using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;
public class GetLiveVacanciesQueryHandler : IRequestHandler<GetLiveVacanciesQuery, GetLiveVacanciesQueryResult>
{
    private readonly IRecruitApiClient<RecruitApiConfiguration> _recruitApiClient;
    private readonly ILiveVacancyMapper _liveVacancyMapper;
    private readonly ICourseService _courseService;

    public GetLiveVacanciesQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, ILiveVacancyMapper liveVacancyMapper, ICourseService courseService)
    {
        _recruitApiClient = recruitApiClient;
        _courseService = courseService;
        _liveVacancyMapper = liveVacancyMapper;
    }

    public async Task<GetLiveVacanciesQueryResult> Handle(GetLiveVacanciesQuery request, CancellationToken cancellationToken)
    {
        var vacanciesResponseTask = _recruitApiClient.GetWithResponseCode<GetLiveVacanciesApiResponse>(new GetLiveVacanciesApiRequest(request.PageNumber, request.PageSize));
        var standardsTask = _courseService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));

        await Task.WhenAll(vacanciesResponseTask, standardsTask);

        var vacanciesResponse = vacanciesResponseTask.Result;
        var standards = standardsTask.Result;
        
        vacanciesResponse.Body.Vacancies = RemoveTraineeships(vacanciesResponse.Body);

        return new GetLiveVacanciesQueryResult
        {
            PageSize = vacanciesResponse.Body.PageSize,
            PageNo = vacanciesResponse.Body.PageNo,
            TotalLiveVacanciesReturned = vacanciesResponse.Body.TotalLiveVacanciesReturned,
            TotalLiveVacancies = vacanciesResponse.Body.TotalLiveVacancies,
            TotalPages = vacanciesResponse.Body.TotalPages,
            Vacancies = vacanciesResponse.Body.Vacancies.Select(x => _liveVacancyMapper.Map(x, standards))
        };
    }

    private static IEnumerable<LiveVacancy> RemoveTraineeships(GetLiveVacanciesApiResponse response)
    {
        return response.Vacancies.Select(x => x)
            .Where(x => x.VacancyType == VacancyType.Apprenticeship)
            .ToList();
    }
}
