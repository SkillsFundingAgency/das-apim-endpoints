using Newtonsoft.Json;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class GetTrainingCourseApiResponse
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("applicationId")]
    public Guid ApplicationId { get; set; }
    [JsonProperty("courseName")]
    public string CourseName { get; set; }
    [JsonProperty("yearAchieved")]
    public int YearAchieved { get; set; }
}
