using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Queries.GetAggregatedCourseDemandList
{
    public class GetAggregatedCourseDemandListQueryHandler : IRequestHandler<GetAggregatedCourseDemandListQuery, GetAggregatedCourseDemandListResult> 
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _demandApiClient;
        private readonly ILocationLookupService _locationLookupService;
        private readonly ICacheStorageService _cacheStorageService;

        public GetAggregatedCourseDemandListQueryHandler(
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            IEmployerDemandApiClient<EmployerDemandApiConfiguration> demandApiClient,
            ILocationLookupService locationLookupService,
            ICacheStorageService cacheStorageService)
        {
            _coursesApiClient = coursesApiClient;
            _demandApiClient = demandApiClient;
            _locationLookupService = locationLookupService;
            _cacheStorageService = cacheStorageService;
        }

        public async Task<GetAggregatedCourseDemandListResult> Handle(GetAggregatedCourseDemandListQuery request, CancellationToken cancellationToken)
        {
            var locationTask = _locationLookupService.GetLocationInformation(request.LocationName, default, default);
            var coursesTask = GetCourses();
            var routesTask = GetRoutes();

            await Task.WhenAll(locationTask, coursesTask, routesTask);
            
            var aggregatedDemands = await _demandApiClient.Get<GetAggregatedCourseDemandListResponse>(
                new GetAggregatedCourseDemandListRequest(
                    request.Ukprn, 
                    request.CourseId,
                    locationTask.Result?.GeoPoint?.FirstOrDefault(),
                    locationTask.Result?.GeoPoint?.LastOrDefault(),
                    locationTask.Result == null ? null : request.LocationRadius,
                    request.Routes));

            return new GetAggregatedCourseDemandListResult
            {
                Courses = coursesTask.Result.Standards,
                AggregatedCourseDemands = aggregatedDemands.AggregatedCourseDemandList,
                Total = aggregatedDemands.Total,
                TotalFiltered = aggregatedDemands.TotalFiltered,
                LocationItem = locationTask.Result,
                Routes = routesTask.Result.Routes.ToList()
            };
        }
        
        private async Task<GetStandardsListResponse> GetCourses()
        {
            var response = await _cacheStorageService.RetrieveFromCache<GetStandardsListResponse>(nameof(GetStandardsListResponse));

            if (response == null)
            {
                response = await _coursesApiClient.Get<GetStandardsListResponse>(new GetAvailableToStartStandardsListRequest());

                await _cacheStorageService.SaveToCache(nameof(GetStandardsListResponse), response, 1);
            }

            return response;
        }

        private async Task<GetRoutesListResponse> GetRoutes()
        {
            var response =
                await _cacheStorageService.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse));

            if (response == null)
            {
                response = await _coursesApiClient.Get<GetRoutesListResponse>(new GetRoutesListRequest());

                await _cacheStorageService.SaveToCache(nameof(GetRoutesListResponse), response, 1);
            }

            return response;
        }
    }
}
