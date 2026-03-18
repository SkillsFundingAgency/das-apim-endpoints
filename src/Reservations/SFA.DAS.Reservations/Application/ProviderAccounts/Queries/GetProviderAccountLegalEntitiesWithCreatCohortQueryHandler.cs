using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;

namespace SFA.DAS.Reservations.Application.ProviderAccounts.Queries;

public class GetProviderAccountLegalEntitiesWithCreatCohortQueryHandler(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> apiClient)
    : IRequestHandler<GetProviderAccountLegalEntitiesWithCreatCohortQuery, GetProviderAccountLegalEntitiesWithCreateCohortResult>
{
    public async Task<GetProviderAccountLegalEntitiesWithCreateCohortResult> Handle(GetProviderAccountLegalEntitiesWithCreatCohortQuery request, CancellationToken cancellationToken)
    {
        return await apiClient.Get<GetProviderAccountLegalEntitiesWithCreateCohortResult>(new GetProviderAccountLegalEntitiesRequest(request.Ukprn, new List<Operation> { Operation.CreateCohort }));
    }
}