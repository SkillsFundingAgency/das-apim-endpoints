using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;
public class GetLiveVacanciesQueryHandler : IRequestHandler<GetLiveVacanciesQuery, GetLiveVacanciesQueryResult>
{
    private readonly IRecruitApiClient<RecruitApiConfiguration> _recruitApiClient;
    private readonly ILiveVacancyMapper _liveVacancyMapper;

    public GetLiveVacanciesQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, ILiveVacancyMapper liveVacancyMapper)
    {
        _recruitApiClient = recruitApiClient;
        _liveVacancyMapper = liveVacancyMapper;
    }

    public async Task<GetLiveVacanciesQueryResult> Handle(GetLiveVacanciesQuery request, CancellationToken cancellationToken)
    {
        var response = await _recruitApiClient.GetWithResponseCode<GetLiveVacanciesApiResponse>(new GetLiveVacanciesApiRequest(request.PageNumber, request.PageSize));
        response.Body.Vacancies = RemoveTraineeships(response.Body);

        return new GetLiveVacanciesQueryResult
        {
            PageSize = response.Body.PageSize,
            PageNo = response.Body.PageNo,
            TotalLiveVacanciesReturned = response.Body.TotalLiveVacanciesReturned,
            TotalLiveVacancies = response.Body.TotalLiveVacancies,
            TotalPages = response.Body.TotalPages,
            Vacancies = await Task.WhenAll(response.Body.Vacancies.Select(x => _liveVacancyMapper.Map(x)))
        };
    }

    private static List<LiveVacancy> RemoveTraineeships(GetLiveVacanciesApiResponse response)
    {
        return response.Vacancies.Select(x => x)
            .Where(x => x.VacancyType == VacancyType.Apprenticeship)
            .ToList();
    }
}
