using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourse;
public class GetTrainingCourseQueryResult
{
    public CourseResponse Course { get; set; }

    public static GetTrainingCourseQueryResult From(GetTrainingCourseApiResponse source)
    {
        return source is null
            ? null
            : new GetTrainingCourseQueryResult
            {
                Course = new CourseResponse
                {
                    Id = source.Id,
                    ApplicationId = source.ApplicationId,
                    CourseName = source.CourseName,
                    YearAchieved = source.YearAchieved
                }
            };
    }
}

public class CourseResponse
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string CourseName { get; set; }
    public int YearAchieved { get; set; }
}
