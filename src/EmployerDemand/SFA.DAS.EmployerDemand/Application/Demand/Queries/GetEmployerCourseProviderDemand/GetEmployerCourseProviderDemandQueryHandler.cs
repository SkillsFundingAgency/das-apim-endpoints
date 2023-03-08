using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetEmployerCourseProviderDemand
{
    public class GetEmployerCourseProviderDemandQueryHandler : IRequestHandler<GetEmployerCourseProviderDemandQuery, GetEmployerCourseProviderDemandQueryResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _employerDemandApiClient;
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;
        private readonly ILocationLookupService _locationLookupService;

        public GetEmployerCourseProviderDemandQueryHandler (
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, 
            IEmployerDemandApiClient<EmployerDemandApiConfiguration> employerDemandApiClient,
            ILocationLookupService locationLookupService, 
            IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient)
        {
            _coursesApiClient = coursesApiClient;
            _employerDemandApiClient = employerDemandApiClient;
            _locationLookupService = locationLookupService;
            _roatpCourseManagementApiClient = roatpCourseManagementApiClient;
        }
        public async Task<GetEmployerCourseProviderDemandQueryResult> Handle(GetEmployerCourseProviderDemandQuery request, CancellationToken cancellationToken)
        {
            var courseTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.CourseId));
            var locationTask = _locationLookupService.GetLocationInformation(request.LocationName,0, 0, true);
            var providerCourseInfoTask =
                _roatpCourseManagementApiClient.Get<GetProviderCourseInformation>(
                    new GetProviderCourseInformationRequest(request.Ukprn, request.CourseId));

            await Task.WhenAll(courseTask, locationTask, providerCourseInfoTask);

            var radius = locationTask.Result != null ? request.LocationRadius : null;
            
            var demand = await _employerDemandApiClient.Get<GetEmployerCourseProviderListResponse>(
                new GetCourseProviderDemandsRequest(request.Ukprn, request.CourseId,
                    locationTask.Result?.GeoPoint?.FirstOrDefault(), locationTask.Result?.GeoPoint?.LastOrDefault(),
                    radius));

            return new GetEmployerCourseProviderDemandQueryResult
            {
                Course = courseTask.Result,
                Location = locationTask.Result,
                EmployerCourseDemands = demand.EmployerCourseDemands,
                Total = demand.Total,
                TotalFiltered = demand.TotalFiltered,
                ProviderDetail = providerCourseInfoTask.Result
            };
        }
    }
}
