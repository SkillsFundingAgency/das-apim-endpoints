using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourse;
public class GetDeleteTrainingCourseQueryResult
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string CourseName { get; set; }
    public int YearAchieved { get; set; }

    public static implicit operator GetDeleteTrainingCourseQueryResult(GetDeleteTrainingCourseResponse source)
    {
        return new GetDeleteTrainingCourseQueryResult
        {
            Id = source.Id,
            ApplicationId = source.ApplicationId,
            CourseName = source.CourseName,
            YearAchieved = source.YearAchieved
        };
    }
}
