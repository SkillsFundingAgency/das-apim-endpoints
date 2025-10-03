using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetAlertsByUkprn;
public class GetAlertsByUkprnQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) :
    IRequestHandler<GetAlertsByUkprnQuery, GetAlertsByUkprnQueryResult>
{
    public async Task<GetAlertsByUkprnQueryResult> Handle(GetAlertsByUkprnQuery request, CancellationToken cancellationToken)
    {
        var alertsResponse = await recruitApiClient.Get<GetProviderAlertsApiResponse>(
            new GetProviderAlertsApiRequest(request.Ukprn, request.UserId));

        return GetAlertsByUkprnQueryResult.FromResponses(alertsResponse);
    }
}