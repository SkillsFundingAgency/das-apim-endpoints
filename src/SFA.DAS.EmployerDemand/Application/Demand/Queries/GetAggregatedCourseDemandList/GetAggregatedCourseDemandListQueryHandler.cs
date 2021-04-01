using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetAggregatedCourseDemandList
{
    public class GetAggregatedCourseDemandListQueryHandler : IRequestHandler<GetAggregatedCourseDemandListQuery, GetAggregatedCourseDemandListResult> 
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _demandApiClient;
        private readonly ILocationLookupService _locationLookupService;

        public GetAggregatedCourseDemandListQueryHandler(
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            IEmployerDemandApiClient<EmployerDemandApiConfiguration> demandApiClient,
            ILocationLookupService locationLookupService)
        {
            _coursesApiClient = coursesApiClient;
            _demandApiClient = demandApiClient;
            _locationLookupService = locationLookupService;
        }

        public async Task<GetAggregatedCourseDemandListResult> Handle(GetAggregatedCourseDemandListQuery request, CancellationToken cancellationToken)
        {
            var locationTask = _locationLookupService.GetLocationInformation(request.LocationName, default, default);
            var coursesTask = _coursesApiClient.Get<GetStandardsListResponse>(new GetAllStandardsListRequest());

            await Task.WhenAll(locationTask, coursesTask);

            var aggregatedDemands = await _demandApiClient.Get<GetAggregatedCourseDemandListResponse>(
                new GetAggregatedCourseDemandListRequest(
                    request.Ukprn, 
                    request.CourseId,
                    locationTask.Result?.GeoPoint?.FirstOrDefault(),
                    locationTask.Result?.GeoPoint?.LastOrDefault()));

            return new GetAggregatedCourseDemandListResult
            {
                Courses = coursesTask.Result.Courses,
                AggregatedCourseDemands = aggregatedDemands.AggregatedCourseDemandList,
                Total = aggregatedDemands.Total,
                TotalFiltered = aggregatedDemands.TotalFiltered
            };
        }
    }
}
