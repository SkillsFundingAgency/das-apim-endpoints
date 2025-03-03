using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.Approvals.Application.ProviderPermissions.Queries;
public class GetAccountProviderLegalEntitiesQueryHandler(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> apiClient) : IRequestHandler<GetAccountProviderLegalEntitiesQuery, GetProviderAccountLegalEntitiesResponse>
{
    public async Task<GetProviderAccountLegalEntitiesResponse> Handle(GetAccountProviderLegalEntitiesQuery request, CancellationToken cancellationToken)
    {
        var apiRequest = new GetProviderAccountLegalEntitiesRequest(request.Ukprn, [Operation.CreateCohort]);

        return await apiClient.Get<GetProviderAccountLegalEntitiesResponse>(apiRequest);
    }
}
