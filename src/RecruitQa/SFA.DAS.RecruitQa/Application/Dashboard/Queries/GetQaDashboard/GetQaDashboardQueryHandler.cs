using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetQaDashboard;
public class GetQaDashboardQueryHandler(IRecruitApiClient recruitApiClient)
    : IRequestHandler<GetQaDashboardQuery, GetQaDashboardQueryResult>
{
    public async Task<GetQaDashboardQueryResult> Handle(GetQaDashboardQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.Get<QaDashboard>(new GetVacancyreviewsQaDashboardApiRequest());
        return GetQaDashboardQueryResult.FromInnerApiResponse(response);
    }
}