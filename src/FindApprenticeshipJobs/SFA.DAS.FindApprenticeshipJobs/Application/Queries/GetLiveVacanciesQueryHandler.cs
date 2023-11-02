using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;
public class GetLiveVacanciesQueryHandler : IRequestHandler<GetLiveVacanciesQuery, GetLiveVacanciesQueryResult>
{
    private readonly IRecruitApiClient<RecruitApiConfiguration> _recruitApiClient;

    public GetLiveVacanciesQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    {
        _recruitApiClient = recruitApiClient;
    }

    public async Task<GetLiveVacanciesQueryResult> Handle(GetLiveVacanciesQuery request, CancellationToken cancellationToken)
    {
        var response = await _recruitApiClient.Get<GetLiveVacanciesApiResponse>(new GetLiveVacanciesApiRequest(request.PageNumber, request.PageSize));

        return new GetLiveVacanciesQueryResult()
        {
            Vacancies = response.Vacancies,
            PageSize = response.PageSize,
            PageNo = response.PageNo,
            TotalLiveVacanciesReturned = response.TotalLiveVacanciesReturned,
            TotalLiveVacancies = response.TotalLiveVacancies,
            TotalPages = response.TotalPages
        };
    }
}
