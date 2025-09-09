using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetQaDashboard;
public class GetQaDashboardQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    : IRequestHandler<GetQaDashboardQuery, GetQaDashboardQueryResult>
{
    public async Task<GetQaDashboardQueryResult> Handle(GetQaDashboardQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.Get<GetQaDashboardApiResponse>(new GetQaDashboardApiRequest());
        return GetQaDashboardQueryResult.FromInnerApiResponse(response);
    }
}