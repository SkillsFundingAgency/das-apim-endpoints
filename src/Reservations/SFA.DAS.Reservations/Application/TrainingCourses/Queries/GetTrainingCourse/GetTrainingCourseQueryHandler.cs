using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.Application.TrainingCourses.Queries.GetTrainingCourse
{
    public class GetTrainingCourseQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        : IRequestHandler<GetTrainingCourseQuery, StandardDetailResponse>
    {
        public Task<StandardDetailResponse> Handle(GetTrainingCourseQuery request, CancellationToken cancellationToken)
        {
            return coursesApiClient.Get<StandardDetailResponse>(new GetStandardDetailsByIdRequest(request.Id));
        }
    }
}