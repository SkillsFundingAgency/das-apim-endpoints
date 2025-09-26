using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardByAccountId
{
    public class GetDashboardByAccountIdQueryHandler(
        IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) 
        : IRequestHandler<GetDashboardByAccountIdQuery, GetDashboardByAccountIdQueryResult>
    {
        public async Task<GetDashboardByAccountIdQueryResult> Handle(GetDashboardByAccountIdQuery request, CancellationToken cancellationToken)
        {
            var dashboardCountTask = recruitApiClient.Get<GetEmployerDashboardApiResponse>(
                new GetDashboardByAccountIdApiRequest(request.AccountId));
            var dashboardAlertsTask = recruitApiClient.Get<GetEmployerAlertsApiResponse>(
                new GetEmployerAlertsApiRequest(request.AccountId, request.UserId));

            await Task.WhenAll(dashboardCountTask, dashboardAlertsTask);

            var dashboardCount = await dashboardCountTask;
            var dashboardAlerts = await dashboardAlertsTask;

            return GetDashboardByAccountIdQueryResult.FromResponses(dashboardCount, dashboardAlerts);
        }
    }
}