using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderPermissions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.Application.ProviderAccounts.Queries;

public class GetProviderAccountLegalEntitiesWithCreatCohortQueryHandler(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> apiClient)
    : IRequestHandler<GetProviderAccountLegalEntitiesWithCreatCohortQuery, GetProviderAccountLegalEntitiesWithCreateCohortResult>
{
    public async Task<GetProviderAccountLegalEntitiesWithCreateCohortResult> Handle(GetProviderAccountLegalEntitiesWithCreatCohortQuery request, CancellationToken cancellationToken)
    {
        return await apiClient.Get<GetProviderAccountLegalEntitiesWithCreateCohortResult>(new GetProviderAccountLegalEntitiesWithCreatCohortRequest(request.Ukprn));
    }
}