using Newtonsoft.Json;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
public record GetTotalPositionsAvailableResponse
{
    [JsonProperty("totalPositionsAvailable")]
    public int TotalPositionsAvailable { get; set; } = 0;
}