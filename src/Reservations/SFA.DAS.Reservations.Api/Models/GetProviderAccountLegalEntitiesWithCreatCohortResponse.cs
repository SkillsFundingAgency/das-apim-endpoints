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
            AccountProviderLegalEntities = source.AccountProviderLegalEntities.Select(item => new GetProviderAccountLegalEntitiesWithCreatCohortResponseItem
            {
                AccountId = item.AccountId,
                AccountLegalEntityId = item.AccountLegalEntityId,
                AccountName = item.AccountName,
                AccountHashedId = item.AccountHashedId,
                AccountProviderId = item.AccountProviderId,
                AccountLegalEntityName = item.AccountLegalEntityName,
                AccountPublicHashedId = item.AccountPublicHashedId,
                AccountLegalEntityPublicHashedId = item.AccountLegalEntityPublicHashedId
            })
        };
    }
}

public class GetProviderAccountLegalEntitiesWithCreatCohortResponseItem
{
    [JsonPropertyName("AccountId")]
    public long AccountId { get; set; }
    
    [JsonPropertyName("accountHashedId")]
    public string AccountHashedId { get; set; }

    [JsonPropertyName("accountPublicHashedId")]
    public string AccountPublicHashedId { get; set; }

    [JsonPropertyName("accountName")]
    public string AccountName { get; set; }

    [JsonPropertyName("accountLegalEntityId")]
    public long AccountLegalEntityId { get; set; }

    [JsonPropertyName("accountLegalEntityPublicHashedId")]
    public string AccountLegalEntityPublicHashedId { get; set; }

    [JsonPropertyName("accountLegalEntityName")]
    public string AccountLegalEntityName { get; set; }

    [JsonPropertyName("accountProviderId")]
    public long AccountProviderId { get; set; }
}