using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Assessors.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Assessors.Application.Queries.GetTrainingCourses
{
    public class GetActiveTrainingCoursesQueryHandler : IRequestHandler<GetActiveTrainingCoursesQuery, GetTrainingCoursesResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetActiveTrainingCoursesQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }
        public async Task<GetTrainingCoursesResult> Handle(GetActiveTrainingCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await _coursesApiClient.Get<GetStandardsListResponse>(new GetActiveStandardsListRequest());

            return new GetTrainingCoursesResult
            {
                TrainingCourses = courses.Standards
            };
        }
    }
}