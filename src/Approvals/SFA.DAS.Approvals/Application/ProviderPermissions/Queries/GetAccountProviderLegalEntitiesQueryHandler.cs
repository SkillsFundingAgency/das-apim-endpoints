using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.ProviderPermissions.Queries;
public class GetAccountProviderLegalEntitiesQueryHandler(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> apiClient) : IRequestHandler<GetAccountProviderLegalEntitiesQuery, GetProviderAccountLegalEntitiesResponse>
{
    public async Task<GetProviderAccountLegalEntitiesResponse> Handle(GetAccountProviderLegalEntitiesQuery request, CancellationToken cancellationToken)
    {
        var apiRequest = new GetProviderAccountLegalEntitiesRequest(request.Ukprn, request.Operations.ToList());

        return await apiClient.Get<GetProviderAccountLegalEntitiesResponse>(apiRequest);
    }
}
