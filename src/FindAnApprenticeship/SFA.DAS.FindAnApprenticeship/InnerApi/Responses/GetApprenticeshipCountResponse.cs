using System.Text.Json.Serialization;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Responses
{
    
    public  class GetApprenticeshipCountResponse
    {
        [JsonPropertyName("totalVacancies")]
        public long TotalVacancies { get; set; }
    }

}