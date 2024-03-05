using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Authorization;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Authorization;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Authorization.Queries;

public class GetCohortAccessQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient) : IRequestHandler<GetCohortAccessQuery, bool>
{
    public async Task<bool> Handle(GetCohortAccessQuery request, CancellationToken cancellationToken)
    {
        var apiRequest = new GetCohortAccessRequest(request.Party, request.PartyId, request.CohortId);

        var response = await apiClient.GetWithResponseCode<GetCohortAccessResponse>(apiRequest);

        response.EnsureSuccessStatusCode();

        return response.Body.HasCohortAccess;
    }
}