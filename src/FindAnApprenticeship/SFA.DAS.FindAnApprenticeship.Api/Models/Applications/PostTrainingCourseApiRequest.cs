using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class PostTrainingCourseApiRequest
{
    public Guid CandidateId { get; set; }
    public string CourseName { get; set; }
    public int YearAchieved { get; set; }
}
