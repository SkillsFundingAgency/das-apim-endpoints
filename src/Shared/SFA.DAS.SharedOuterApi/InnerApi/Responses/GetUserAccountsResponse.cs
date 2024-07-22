using System;
using System.Text.Json.Serialization;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses;

public class GetUserAccountsResponse
{
    [JsonPropertyName(nameof(AccountId))]
    public long AccountId { get; set; }
    
    [JsonPropertyName(nameof(DateRegistered))]
    public DateTime? DateRegistered { get; set; }
    
    [JsonPropertyName("HashedAccountId")] 
    public string EncodedAccountId { get; set; }

    [JsonPropertyName("DasAccountName")] 
    public string DasAccountName { get; set; }

    [JsonPropertyName("Role")] 
    public string Role { get; set; }

    [JsonPropertyName("EmployerType")] 
    public ApprenticeshipEmployerType ApprenticeshipEmployerType { get; set; }

    [JsonPropertyName(nameof(NameConfirmed))] 
    public bool NameConfirmed { get; set; }
}