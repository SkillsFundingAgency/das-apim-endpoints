using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
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
            var courseLevelsTask = _courseService.GetLevels();

            await Task.WhenAll(locationTask, routesTask, courseLevelsTask);

            var location = locationTask.Result;
            var routes = routesTask.Result;
            var courseLevels = courseLevelsTask.Result;
            
            if (request.SearchTerm != null && Regex.IsMatch(request.SearchTerm, @"^VAC\d{10}$", RegexOptions.None, TimeSpan.FromSeconds(3)))
            {
                var vacancyReferenceResult = await _findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(new GetVacancyRequest(request.SearchTerm));

                if (vacancyReferenceResult != null)
                {
                    return new SearchApprenticeshipsResult
                    {
                        TotalApprenticeshipCount = 1,
                        LocationItem = location,
                        Routes = routes.Routes.ToList(),
                        Vacancies = new List<GetVacanciesListItem>(),
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize,
                        TotalPages = 1,
                        VacancyReference = request.SearchTerm,
                        Levels = courseLevels.Levels.ToList()
                    };
                }
            }

            var categories = routes.Routes.Where(route => request.SelectedRouteIds != null && request.SelectedRouteIds.Contains(route.Id.ToString()))
                .Select(route => route.Name).ToList();

            var vacancyResult = await _findApprenticeshipApiClient.Get<GetVacanciesResponse>(
                new GetVacanciesRequest(
                    location?.GeoPoint?.FirstOrDefault(),
                    location?.GeoPoint?.LastOrDefault(),
                    request.Distance,
                    request.SearchTerm,
                    request.PageNumber,
                    request.PageSize,
                    categories,
                    request.SelectedLevelIds,
                    request.Sort));

            var totalPages = (int)Math.Ceiling((double)vacancyResult.TotalFound / request.PageSize);

            return new SearchApprenticeshipsResult
            {
                TotalApprenticeshipCount = vacancyResult.Total,
                TotalFound = vacancyResult.TotalFound,
                LocationItem = location,
                Routes = routes.Routes.ToList(),
                Vacancies = vacancyResult.ApprenticeshipVacancies.ToList(),
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = totalPages,
                Levels = courseLevels.Levels.ToList()
            };
        }
    }
}