using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ProviderRelationships;

public class GetProviderAccountLegalEntitiesWithCreatCohortResponse
{
    [JsonPropertyName("AccountProviderLegalEntities")]
    public List<GetProviderAccountLegalEntityWithCreatCohortItem> AccountProviderLegalEntities { get; set; }
}

public class GetProviderAccountLegalEntityWithCreatCohortItem
{
    [JsonPropertyName("AccountId")]
    public long AccountId { get; set; }
}