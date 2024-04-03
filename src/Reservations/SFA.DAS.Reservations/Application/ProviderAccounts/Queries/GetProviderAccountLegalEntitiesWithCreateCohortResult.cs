using System.Collections.Generic;

namespace SFA.DAS.Reservations.Application.ProviderAccounts.Queries;

public record GetProviderAccountLegalEntitiesWithCreateCohortResult
{
    public List<GetProviderAccountLegalEntitiesWithCreateCohortResultItem> AccountProviderLegalEntities { get ; set ; }
}

public class GetProviderAccountLegalEntitiesWithCreateCohortResultItem
{
    public long AccountId { get; set; }
}