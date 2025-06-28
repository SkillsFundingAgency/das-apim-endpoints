using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardVacanciesCountByAccountId;
public class GetDashboardVacanciesCountByAccountIdQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    : IRequestHandler<GetDashboardVacanciesCountByAccountIdQuery, GetDashboardVacanciesCountByAccountIdQueryResult>
{
    public async Task<GetDashboardVacanciesCountByAccountIdQueryResult> Handle(GetDashboardVacanciesCountByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.Get<GetDashboardVacanciesCountApiResponse>(
            new GetDashboardVacanciesCountByAccountIdApiRequest(request.AccountId,
                request.PageNumber,
                request.PageSize,
                request.SortColumn,
                request.IsAscending,
                request.Status));

        return response;
    }
}