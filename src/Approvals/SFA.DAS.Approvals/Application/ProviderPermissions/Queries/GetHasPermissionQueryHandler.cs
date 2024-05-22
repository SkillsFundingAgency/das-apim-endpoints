using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderPermissions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.ProviderPermissions.Queries;

public class GetHasPermissionQueryHandler(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> apiClient) : IRequestHandler<GetHasPermissionQuery, bool>
{
    public async Task<bool> Handle(GetHasPermissionQuery request, CancellationToken cancellationToken)
    {
        var apiRequest = new GetHasPermissionRequest(request.Ukprn, request.AccountLegalEntityId, request.Operation);
        
        return await apiClient.Get<bool>(apiRequest);
    }
}