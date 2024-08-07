using System;
using Newtonsoft.Json;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAgreements;

public record GetEmployerAgreementsResponse
{
    [JsonProperty(nameof(Id))]
    public long Id { get; set; }
    [JsonProperty(nameof(AccountId))]
    public long AccountId { get; set; }
    [JsonProperty(nameof(Acknowledged))]
    public bool? Acknowledged { get; set; }
    [JsonProperty(nameof(SignedDate))]
    public DateTime? SignedDate { get; set; }
};