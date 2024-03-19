using System.Text.Json.Serialization;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

public class PutQualificationApiResponse
{
    [JsonPropertyName("qualification")]
    public Qualification Qualification { get; set; }
}