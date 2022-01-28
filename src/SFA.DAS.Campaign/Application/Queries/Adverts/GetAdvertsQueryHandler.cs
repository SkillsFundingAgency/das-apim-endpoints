using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.Adverts
{
    public class GetAdvertsQueryHandler : IRequestHandler<GetAdvertsQuery, GetAdvertsQueryResult>
    {
        private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;
        private readonly ICourseService _courseService;
        private readonly ILocationLookupService _locationLookupService;

        public GetAdvertsQueryHandler (
            IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,  
            ICourseService courseService, 
            ILocationLookupService locationLookupService)
        {
            _findApprenticeshipApiClient = findApprenticeshipApiClient;
            _courseService = courseService;
            _locationLookupService = locationLookupService;
        }
        
        public async Task<GetAdvertsQueryResult> Handle(GetAdvertsQuery request, CancellationToken cancellationToken)
        {
            var routesTask = _courseService.GetRoutes();
            var locationTask = _locationLookupService.GetLocationInformation(request.Postcode, 0, 0);

            await Task.WhenAll(routesTask, locationTask);

            if (locationTask.Result == null)
            {
                return new GetAdvertsQueryResult
                {
                    Routes = routesTask.Result.Routes,
                    Vacancies = new List<GetVacanciesListItem>()
                };
            }

            var categories = _courseService.MapRoutesToCategories(new List<string> { request.Route });

            var advertRequest = new GetVacanciesRequest(0, 20, null, null, null, null, null,
                locationTask.Result.GeoPoint.FirstOrDefault(), locationTask.Result.GeoPoint.LastOrDefault(),
                request.Distance, categories, null, "DistanceDesc");

            var adverts = await _findApprenticeshipApiClient.Get<GetVacanciesResponse>(advertRequest);
            
            return new GetAdvertsQueryResult
            {
                Location = locationTask.Result,
                Routes = routesTask.Result.Routes,
                TotalFound = adverts.TotalFound,
                Vacancies = adverts.ApprenticeshipVacancies
            };
        }
    }
}