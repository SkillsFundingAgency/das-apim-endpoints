using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships
{
    public class SearchApprenticeshipsQueryHandler : IRequestHandler<SearchApprenticeshipsQuery, SearchApprenticeshipsResult>
    {
        private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;
        private readonly ILocationLookupService _locationLookupService;
        private readonly ICourseService _courseService;

        public SearchApprenticeshipsQueryHandler(IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient, ILocationLookupService locationLookupService, ICourseService courseService)
        {
            _findApprenticeshipApiClient = findApprenticeshipApiClient;
            _locationLookupService = locationLookupService;
            _courseService = courseService;
        }

        public async Task<SearchApprenticeshipsResult> Handle(SearchApprenticeshipsQuery request, CancellationToken cancellationToken)
        {
            var locationTask = _locationLookupService.GetLocationInformation(request.Location, default, default, false);
            var routesTask = _courseService.GetRoutes();

            await Task.WhenAll(locationTask, routesTask);
            
            var location = locationTask.Result;
            var routes = routesTask.Result;
            
            var result = await _findApprenticeshipApiClient.Get<GetApprenticeshipCountResponse>(
                    new GetApprenticeshipCountRequest(
                        location?.GeoPoint?.FirstOrDefault(),
                        location?.GeoPoint?.LastOrDefault(),
                        request.SelectedRouteIds,
                        request.Distance,
                        request.WhatSearchTerm
                        ));

            var vacancyResult = await _findApprenticeshipApiClient.Get<GetVacanciesResponse>(
                new GetVacanciesRequest(
                    location?.GeoPoint?.FirstOrDefault(),
                    location?.GeoPoint?.LastOrDefault(),
                    request.SelectedRouteIds,
                    request.Distance,
                    request.Sort,
                    request.WhatSearchTerm
                ));

            return new SearchApprenticeshipsResult
            {
                TotalApprenticeshipCount = result.TotalVacancies,
                LocationItem = location,
                Routes = routes.Routes.ToList(),
                Vacancies = vacancyResult.ApprenticeshipVacancies.ToList()
            };
        }
    }
}