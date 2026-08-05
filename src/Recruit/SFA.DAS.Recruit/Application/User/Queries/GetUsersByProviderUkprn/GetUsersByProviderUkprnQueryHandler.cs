using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Recruit.Application.User.Queries.GetUsersByProviderUkprn;

public class GetUsersByProviderUkprnQueryHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient) : IRequestHandler<GetUsersByProviderUkprnQuery, GetUsersByProviderUkprnQueryResult>
{
    public async Task<GetUsersByProviderUkprnQueryResult> Handle(GetUsersByProviderUkprnQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.GetWithResponseCode<GetUsersByProviderUkprnResponse>(new GetUsersByProviderUkprnRequest(request.Ukprn));
        return response.StatusCode == HttpStatusCode.NotFound
            ? new GetUsersByProviderUkprnQueryResult(null)
            : new GetUsersByProviderUkprnQueryResult(response.Body);
    }
}