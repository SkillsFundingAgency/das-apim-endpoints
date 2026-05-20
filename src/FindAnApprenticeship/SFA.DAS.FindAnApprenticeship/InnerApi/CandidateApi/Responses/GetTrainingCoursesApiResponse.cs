using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class GetTrainingCoursesApiResponse
{
    [JsonProperty("trainingCourses")]
    public List<GetTrainingCourseApiResponse> TrainingCourses { get; set; }
}
