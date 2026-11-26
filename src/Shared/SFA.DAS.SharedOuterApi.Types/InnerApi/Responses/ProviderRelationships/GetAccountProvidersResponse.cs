using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;
using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ProviderRelationships;

public class GetAccountProvidersResponse
{
    [JsonPropertyName("AccountId")]
    public long AccountId { get; set; }

    [JsonPropertyName("AccountProviders")]
    public List<AccountProviderResponse> AccountProviders { get; set; }
}

public class AccountProviderResponse
{
    public long Ukprn { get; set; }
    public string ProviderName { get; set; }
    public List<AccountLegalEntityModel> AccountLegalEntities { get; set; } = [];
}

public class AccountLegalEntityModel
{
    public string? PublicHashedId { get; set; }
    public string? Name { get; set; }
    public List<Operation> Operations { get; set; } = [];
}