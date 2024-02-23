using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourse;
public class GetDeleteTrainingCourseQueryResult
{
    public CourseResponse Course { get; set; }

    public static implicit operator GetDeleteTrainingCourseQueryResult(GetTrainingCourseApiResponse source)
    {
        return new GetDeleteTrainingCourseQueryResult
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
