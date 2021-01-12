using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries
{
    public class GetFrameworksQueryHandler : IRequestHandler<GetFrameworksQuery, GetFrameworksResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetFrameworksQueryHandler (ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }
        public async Task<GetFrameworksResult> Handle(GetFrameworksQuery request, CancellationToken cancellationToken)
        {
            var response = await _coursesApiClient.Get<GetFrameworksListResponse>(new GetFrameworksRequest());
            
            return new GetFrameworksResult
            {
                Frameworks = response.Frameworks
            };
                
        }
    }
}