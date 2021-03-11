using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRegisterDemand
{
    public class GetRegisterDemandQueryHandler : IRequestHandler<GetRegisterDemandQuery, GetRegisterDemandResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetRegisterDemandQueryHandler (ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }
        public async Task<GetRegisterDemandResult> Handle(GetRegisterDemandQuery request, CancellationToken cancellationToken)
        {
            var course = await _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.CourseId));
            
            return new GetRegisterDemandResult
            {
                Course = course
            };
        }
    }
}