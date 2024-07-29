﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.ProviderPermissions.Queries;

public class GetHasRelationshipWithPermissionQueryHandler(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> apiClient) : IRequestHandler<GetHasRelationshipWithPermissionQuery, bool>
{
    public async Task<bool> Handle(GetHasRelationshipWithPermissionQuery request, CancellationToken cancellationToken)
    {
        var apiRequest = new GetHasRelationshipWithPermissionRequest(request.Ukprn, request.Operation);

        return await apiClient.Get<bool>(apiRequest);
    }
}