using System.Text.Json.Serialization;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

public class PutApplicationQualificationApiResponse
{
    [JsonPropertyName("qualification")]
    public Qualification Qualification { get; set; }
}