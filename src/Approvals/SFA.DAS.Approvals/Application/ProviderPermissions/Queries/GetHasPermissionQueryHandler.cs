using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.Approvals.Application.ProviderPermissions.Queries;

public class GetHasPermissionQueryHandler(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> _apiClient) : IRequestHandler<GetHasPermissionQuery, bool>
{
    public async Task<bool> Handle(GetHasPermissionQuery request, CancellationToken cancellationToken)
    {
        var apiRequest = new GetHasPermissionRequest(request.Ukprn, request.AccountLegalEntityId, Operation.CreateCohort);

        return await _apiClient.Get<bool>(apiRequest);
    }
}
