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
            return await recruitApiClient.Get<GetDashboardApiResponse>(
                new GetDashboardByUkprnApiRequest(request.Ukprn));
        }
    }
}