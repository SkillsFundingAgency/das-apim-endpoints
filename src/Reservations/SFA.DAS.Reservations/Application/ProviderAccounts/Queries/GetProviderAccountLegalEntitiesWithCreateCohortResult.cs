using System.Collections.Generic;

namespace SFA.DAS.Reservations.Application.ProviderAccounts.Queries;

public record GetProviderAccountLegalEntitiesWithCreateCohortResult
{
    public List<GetProviderAccountLegalEntitiesWithCreateCohortResultItem> AccountProviderLegalEntities { get ; set ; }
}

public class GetProviderAccountLegalEntitiesWithCreateCohortResultItem
{
    public long AccountId { get; set; }
    public string AccountHashedId { get; set; }
    public string AccountPublicHashedId { get; set; }
    public string AccountName { get; set; }
    public int AccountLegalEntityId { get; set; }
    public string AccountLegalEntityPublicHashedId { get; set; }
    public string AccountLegalEntityName { get; set; }
    public long AccountProviderId { get; set; }
}