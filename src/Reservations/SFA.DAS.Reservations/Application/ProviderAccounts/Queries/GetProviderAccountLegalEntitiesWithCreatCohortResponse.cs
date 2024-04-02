using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.Reservations.Application.ProviderAccounts.Queries;

public record GetProviderAccountLegalEntitiesWithCreatCohortResponse
{
    public List<GetProviderAccountLegalEntityWithCreatCohortItem> ProviderAccountLegalEntities { get ; set ; }
}

public class GetProviderAccountLegalEntityWithCreatCohortItem
{
    [JsonPropertyName("AccountId")]
    public long AccountId { get; set; }
}