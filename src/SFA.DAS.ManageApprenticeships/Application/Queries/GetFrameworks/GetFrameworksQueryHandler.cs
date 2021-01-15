using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ManageApprenticeships.InnerApi.Requests;
using SFA.DAS.ManageApprenticeships.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.GetFrameworks
{
    public class GetFrameworksQueryHandler : IRequestHandler<GetFrameworksQuery, GetFrameworksQueryResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetFrameworksQueryHandler (ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }
        public async Task<GetFrameworksQueryResult> Handle(GetFrameworksQuery request, CancellationToken cancellationToken)
        {
            var response = await _coursesApiClient.Get<GetFrameworksListResponse>(new GetFrameworksRequest());
            
            return new GetFrameworksQueryResult
            {
                Frameworks = response.Frameworks
            };
        }
    }
}