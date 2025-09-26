using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.GetDashboardByUkprn
{
    public class GetDashboardByUkprnQueryHandler(
        IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) 
        : IRequestHandler<GetDashboardByUkprnQuery, GetDashboardByUkprnQueryResult>
    {
        public async Task<GetDashboardByUkprnQueryResult> Handle(GetDashboardByUkprnQuery request, CancellationToken cancellationToken)
        {
            var dashboardCountTask = recruitApiClient.Get<GetProviderDashboardApiResponse>(
                new GetDashboardByUkprnApiRequest(request.Ukprn));
            var dashboardAlertsTask = recruitApiClient.Get<GetProviderAlertsApiResponse>(
                new GetProviderAlertsApiRequest(request.Ukprn, request.UserId));

            await Task.WhenAll(dashboardCountTask, dashboardAlertsTask);

            var dashboardCount = await dashboardCountTask;
            var dashboardAlerts = await dashboardAlertsTask;

            return GetDashboardByUkprnQueryResult.FromResponses(dashboardCount, dashboardAlerts);
        }
    }
}