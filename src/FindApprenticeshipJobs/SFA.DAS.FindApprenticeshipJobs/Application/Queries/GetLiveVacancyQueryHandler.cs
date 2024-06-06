using MediatR;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;

public class GetLiveVacancyQueryHandler : IRequestHandler<GetLiveVacancyQuery, GetLiveVacancyQueryResult>
{
    private readonly IRecruitApiClient<RecruitApiConfiguration> _recruitApiClient;
    private readonly ILiveVacancyMapper _liveVacancyMapper;
    private readonly ICourseService _courseService;

    public GetLiveVacancyQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, ILiveVacancyMapper liveVacancyMapper, ICourseService courseService)
    {
        _recruitApiClient = recruitApiClient;
        _liveVacancyMapper = liveVacancyMapper;
        _courseService = courseService;
    }

    public async Task<GetLiveVacancyQueryResult> Handle(GetLiveVacancyQuery request, CancellationToken cancellationToken)
    {
        var responseTask = _recruitApiClient.GetWithResponseCode<GetLiveVacancyApiResponse>(new GetLiveVacancyApiRequest(request.VacancyReference));
        var standardsTask = _courseService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));

        await Task.WhenAll(responseTask, standardsTask);

        var response = responseTask.Result;
        var standards = standardsTask.Result;
        
        var result = _liveVacancyMapper.Map(response.Body, standards);

        return new GetLiveVacancyQueryResult
        {
            LiveVacancy = result
        };

    }
}