using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourse;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.DeleteTrainingCourse;
public class GetDeleteTrainingCourseQueryResult
{
    public CourseResponse Course { get; set; }

    public static GetDeleteTrainingCourseQueryResult From(GetTrainingCourseApiResponse source)
    {
        return source is null
            ? null
            : new GetDeleteTrainingCourseQueryResult
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
