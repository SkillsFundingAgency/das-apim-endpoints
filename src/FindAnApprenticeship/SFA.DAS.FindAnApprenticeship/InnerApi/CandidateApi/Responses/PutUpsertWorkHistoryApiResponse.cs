using Newtonsoft.Json;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class PutUpsertWorkHistoryApiResponse
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("employer")]
    public string Employer { get; set; }
    [JsonProperty("jobTitle")]
    public string JobTitle { get; set; }
    [JsonProperty("startDate")]
    public DateTime StartDate { get; set; }
    [JsonProperty("endDate")]
    public DateTime? EndDate { get; set; }
    [JsonProperty("applicationId")]
    public Guid ApplicationId { get; set; }
    [JsonProperty("description")]
    public string Description { get; set; }
}
