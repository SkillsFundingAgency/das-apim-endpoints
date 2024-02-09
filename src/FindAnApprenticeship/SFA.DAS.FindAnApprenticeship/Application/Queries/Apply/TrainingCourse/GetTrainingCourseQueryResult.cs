using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourse;
public class GetTrainingCourseQueryResult
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string CourseName { get; set; }
    public int YearAchieved { get; set; }

    public static implicit operator GetTrainingCourseQueryResult(GetTrainingCourseApiResponse source)
    {
        return new GetTrainingCourseQueryResult
        {
            Id = source.Id,
            ApplicationId = source.ApplicationId,
            CourseName = source.CourseName,
            YearAchieved = source.YearAchieved
        };
    }
}
