using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SFA.DAS.Reservations.Application.ProviderAccounts.Queries;

namespace SFA.DAS.Reservations.Api.Models;

public record GetProviderAccountLegalEntitiesWithCreatCohortResponse
{
    [JsonPropertyName("AccountProviderLegalEntities")]
    public IEnumerable<GetProviderAccountLegalEntitiesWithCreatCohortResponseItem> AccountProviderLegalEntities { get; set; }
    
    public static implicit operator GetProviderAccountLegalEntitiesWithCreatCohortResponse(GetProviderAccountLegalEntitiesWithCreateCohortResult source)
    {
        return new GetProviderAccountLegalEntitiesWithCreatCohortResponse
        {
            AccountProviderLegalEntities = source.AccountProviderLegalEntities.Select(x => new GetProviderAccountLegalEntitiesWithCreatCohortResponseItem
            {
                AccountId = x.AccountId
            })
        };
    }
}

public class GetProviderAccountLegalEntitiesWithCreatCohortResponseItem
{
    [JsonPropertyName("AccountId")]
    public long AccountId { get; set; }
}