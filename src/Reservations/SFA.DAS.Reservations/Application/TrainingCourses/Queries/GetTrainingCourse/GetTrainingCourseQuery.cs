using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;

namespace SFA.DAS.Reservations.Application.TrainingCourses.Queries.GetTrainingCourse
{
    public class GetTrainingCourseQuery(string id) : IRequest<StandardDetailResponse>
    {
        public string Id { get; set; } = id;
    }
}