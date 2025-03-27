using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.Application.TrainingCourses.Queries.GetTrainingCourseList
{
    public class GetTrainingCoursesQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        : IRequestHandler<GetTrainingCoursesQuery, GetTrainingCoursesResult>
    {
        public async Task<GetTrainingCoursesResult> Handle(GetTrainingCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await coursesApiClient.Get<GetStandardsListResponse>(new GetAvailableToStartStandardsListRequest());
            
            return new GetTrainingCoursesResult
            {
                Courses = courses.Standards
            }; 
                
        }
    }
}