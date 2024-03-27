using System;
using System.Text.Json.Serialization;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

public class QualificationReference
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}