using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetAlertsByAccountId;
public class GetAlertsByAccountIdQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) :
    IRequestHandler<GetAlertsByAccountIdQuery, GetAlertsByAccountIdQueryResult>
{
    public async Task<GetAlertsByAccountIdQueryResult> Handle(GetAlertsByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var alertsResponse = await recruitApiClient.Get<GetEmployerAlertsApiResponse>(
            new GetEmployerAlertsApiRequest(request.AccountId, request.UserId));

        return GetAlertsByAccountIdQueryResult.FromResponses(alertsResponse);
    }
}