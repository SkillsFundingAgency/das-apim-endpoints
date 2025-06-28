using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardVacanciesCountByUkprn;
public class GetDashboardVacanciesCountByUkprnQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    : IRequestHandler<GetDashboardVacanciesCountByUkprnQuery, GetDashboardVacanciesCountByUkprnQueryResult>
{
    public async Task<GetDashboardVacanciesCountByUkprnQueryResult> Handle(GetDashboardVacanciesCountByUkprnQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.Get<GetDashboardVacanciesCountApiResponse>(
            new GetDashboardVacanciesCountByUkprnApiRequest(request.Ukprn,
                request.PageNumber,
                request.PageSize,
                request.SortColumn,
                request.IsAscending,
                request.Status));

        return response;
    }
}