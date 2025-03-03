using System;
using Newtonsoft.Json;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.User;

public record GetUserByRefResponse
{
    [JsonProperty(nameof(Id))]
    public long Id { get; set; }
    [JsonProperty(nameof(Email))]
    public string Email { get; set; }
    [JsonProperty(nameof(FirstName))]
    public string FirstName { get; set; }
    [JsonProperty(nameof(LastName))]
    public string LastName { get; set; }
    [JsonProperty(nameof(CorrelationId))]
    public string CorrelationId { get; set; }
    [JsonProperty(nameof(TermAndConditionsAcceptedOn))]
    public DateTime? TermAndConditionsAcceptedOn { get; set; }
    [JsonProperty(nameof(FullName))]
    public string FullName => $"{FirstName} {LastName}";
};