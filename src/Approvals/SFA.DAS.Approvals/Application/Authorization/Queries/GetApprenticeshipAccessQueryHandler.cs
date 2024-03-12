using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Authorization;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Authorization;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Authorization.Queries;

public class GetApprenticeshipAccessQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient) : IRequestHandler<GetApprenticeshipAccessQuery, bool>
{
    public async Task<bool> Handle(GetApprenticeshipAccessQuery request, CancellationToken cancellationToken)
    {
        var apiRequest = new GetApprenticeshipAccessRequest(request.Party, request.PartyId, request.ApprenticeshipId);
        
        var response = await apiClient.Get<GetApprenticeshipAccessResponse>(apiRequest);
        
        return response.HasApprenticeshipAccess;
    }
}