using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;

namespace SFA.DAS.Approvals.Application.ProviderPermissions.Queries;

public class GetHasRelationshipWithPermissionQueryHandler(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> apiClient) : IRequestHandler<GetHasRelationshipWithPermissionQuery, bool>
{
    public async Task<bool> Handle(GetHasRelationshipWithPermissionQuery request, CancellationToken cancellationToken)
    {
        var apiRequest = new GetHasRelationshipWithPermissionRequest(request.Ukprn, Operation.CreateCohort);

        return await apiClient.Get<bool>(apiRequest);
    }
}