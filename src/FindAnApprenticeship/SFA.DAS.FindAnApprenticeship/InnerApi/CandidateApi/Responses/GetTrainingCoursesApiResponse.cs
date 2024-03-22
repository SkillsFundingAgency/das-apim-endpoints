using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class GetTrainingCoursesApiResponse
{
    [JsonProperty("trainingCourses")]
    public List<TrainingCourseItem> TrainingCourses { get; set; }

    public class TrainingCourseItem
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
}
