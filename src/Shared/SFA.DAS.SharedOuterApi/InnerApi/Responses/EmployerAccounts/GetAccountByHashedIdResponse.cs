using System;
using Newtonsoft.Json;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;

public class GetAccountByHashedIdResponse
{
    [JsonProperty(nameof(AccountId))]
    public long AccountId { get; set; }
    [JsonProperty(nameof(HashedAccountId))]
    public string HashedAccountId { get; set; }
    [JsonProperty(nameof(PublicHashedAccountId))]
    public string PublicHashedAccountId { get; set; }
    [JsonProperty(nameof(DasAccountName))]
    public string DasAccountName { get; set; }
    [JsonProperty(nameof(DateRegistered))]
    public DateTime DateRegistered { get; set; }
    [JsonProperty(nameof(OwnerEmail))]
    public string OwnerEmail { get; set; }
    [JsonProperty(nameof(NameConfirmed))]
    public bool? NameConfirmed { get; set; }
    [JsonProperty(nameof(ApprenticeshipEmployerType))]
    public ApprenticeshipEmployerType ApprenticeshipEmployerType { get; set; }

    public bool AddTrainingProviderAcknowledged { get; set; }
}
