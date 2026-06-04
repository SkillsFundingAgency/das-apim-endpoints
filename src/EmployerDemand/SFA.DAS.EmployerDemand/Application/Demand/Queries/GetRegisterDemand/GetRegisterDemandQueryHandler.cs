using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRegisterDemand
{
    public class GetRegisterDemandQueryHandler : IRequestHandler<GetRegisterDemandQuery, GetRegisterDemandResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ILocationLookupService _locationLookupService;

        public GetRegisterDemandQueryHandler (ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ILocationLookupService locationLookupService)
        {
            _coursesApiClient = coursesApiClient;
            _locationLookupService = locationLookupService;
        }
        public async Task<GetRegisterDemandResult> Handle(GetRegisterDemandQuery request, CancellationToken cancellationToken)
        {
            var course = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.CourseId));
            var location = _locationLookupService.GetLocationInformation(request.LocationName,0,0, true); 
            
            await Task.WhenAll(course, location);
            
            return new GetRegisterDemandResult
            {
                Course = course.Result,
                Location = location.Result
            };
        }
    }
}