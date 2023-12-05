using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;

public class GetLiveVacancyQueryHandler : IRequestHandler<GetLiveVacancyQuery, GetLiveVacancyQueryResult>
{
    private readonly IRecruitApiClient<RecruitApiConfiguration> _recruitApiClient;
    private readonly ILiveVacancyMapper _liveVacancyMapper;

    public GetLiveVacancyQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, ILiveVacancyMapper liveVacancyMapper)
    {
        _recruitApiClient = recruitApiClient;
        _liveVacancyMapper = liveVacancyMapper;
    }

    public async Task<GetLiveVacancyQueryResult> Handle(GetLiveVacancyQuery request, CancellationToken cancellationToken)
    {
        var response = await _recruitApiClient.GetWithResponseCode<GetLiveVacancyApiResponse>(new GetLiveVacancyApiRequest(request.VacancyReference));
        var result = await _liveVacancyMapper.Map(response.Body);

        return new GetLiveVacancyQueryResult
        {
            LiveVacancy = result
        };

    }
}