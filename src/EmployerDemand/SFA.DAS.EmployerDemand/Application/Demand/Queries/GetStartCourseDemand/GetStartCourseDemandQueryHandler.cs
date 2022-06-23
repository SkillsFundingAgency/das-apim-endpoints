using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetStartCourseDemand
{
    public class GetStartCourseDemandQueryHandler : IRequestHandler<GetStartCourseDemandQuery, GetStartCourseDemandQueryResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetStartCourseDemandQueryHandler (ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }
        public async Task<GetStartCourseDemandQueryResult> Handle(GetStartCourseDemandQuery request, CancellationToken cancellationToken)
        {
            var result = await _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.CourseId));

            return new GetStartCourseDemandQueryResult
            {
                Course = result
            };
        }
    }
}