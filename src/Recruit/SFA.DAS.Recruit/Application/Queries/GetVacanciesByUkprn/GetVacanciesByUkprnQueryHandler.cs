using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetVacanciesByUkprn;
public class GetVacanciesByUkprnQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    : IRequestHandler<GetVacanciesByUkprnQuery, GetVacanciesByUkprnQueryResult>
{
    public async Task<GetVacanciesByUkprnQueryResult> Handle(GetVacanciesByUkprnQuery request, CancellationToken cancellationToken)
    {
        var vacanciesResponse = await recruitApiClient.Get<GetPagedVacancySummaryApiResponse>(
            new GetVacanciesByUkprnApiRequest(request.Ukprn,
                request.Page,
                request.PageSize,
                request.SortColumn,
                request.SortOrder,
                request.FilterBy,
                request.SearchTerm));
        return GetVacanciesByUkprnQueryResult.FromResponses(vacanciesResponse);
    }
}