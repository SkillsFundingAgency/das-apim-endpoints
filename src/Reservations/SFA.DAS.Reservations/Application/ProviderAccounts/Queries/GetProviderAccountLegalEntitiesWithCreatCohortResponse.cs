using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRelationships;

namespace SFA.DAS.Reservations.Application.ProviderAccounts.Queries;

public record GetProviderAccountLegalEntitiesWithCreatCohortResponse
{
    public List<GetProviderAccountLegalEntityWithCreatCohortItem> ProviderAccountLegalEntities { get ; set ; }
}