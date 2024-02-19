using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourse;
using System;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

public class GetTrainingCourseApiResponse
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string CourseName { get; set; }
    public int YearAchieved { get; set; }

    public static implicit operator GetTrainingCourseApiResponse(GetTrainingCourseQueryResult source)
    {
        return new GetTrainingCourseApiResponse
        {
            ApplicationId = source.ApplicationId,
            Id = source.Id,
            CourseName = source.CourseName,
            YearAchieved = source.YearAchieved
        };
    }
}
