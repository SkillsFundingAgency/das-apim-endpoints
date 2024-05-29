using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class GetTrainingCoursesApiResponse
{
    [JsonProperty("trainingCourses")]
    public List<GetTrainingCourseApiResponse> TrainingCourses { get; set; }
}
