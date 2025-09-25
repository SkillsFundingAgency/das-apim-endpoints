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
            return await recruitApiClient.Get<GetEmployerDashboardApiResponse>(
                new GetDashboardByAccountIdApiRequest(request.AccountId, request.UserId));
        }
    }
}