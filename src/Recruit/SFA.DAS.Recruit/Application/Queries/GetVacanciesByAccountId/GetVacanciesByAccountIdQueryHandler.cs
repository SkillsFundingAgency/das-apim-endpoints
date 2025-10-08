using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetVacanciesByAccountId;
public class GetVacanciesByAccountIdQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    : IRequestHandler<GetVacanciesByAccountIdQuery, GetVacanciesByAccountIdQueryResult>
{
    public async Task<GetVacanciesByAccountIdQueryResult> Handle(GetVacanciesByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var vacanciesResponse = await recruitApiClient.Get<GetPagedVacancySummaryApiResponse>(
            new GetVacanciesByAccountIdApiRequest(request.AccountId,
                request.Page,
                request.PageSize,
                request.SortColumn,
                request.SortOrder,
                request.FilterBy,
                request.SearchTerm));
        return GetVacanciesByAccountIdQueryResult.FromResponses(vacanciesResponse);
    }
}