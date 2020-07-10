using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Reservations.InnerApi.Requests;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.Reservations.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.Reservations.Application.TrainingCourses.Queries.GetTrainingCourseList
{
    public class GetTrainingCoursesQueryHandler : IRequestHandler<GetTrainingCoursesQuery, GetTrainingCoursesResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetTrainingCoursesQueryHandler (ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }
        public async Task<GetTrainingCoursesResult> Handle(GetTrainingCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await _coursesApiClient.Get<GetStandardsListResponse>(new GetStandardsRequest());
            
            return new GetTrainingCoursesResult
            {
                Courses = courses.Standards
            }; 
                
        }
    }
}