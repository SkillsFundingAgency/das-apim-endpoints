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

            await Task.WhenAll(locationTask, routesTask);

            var location = locationTask.Result;
            var routes = routesTask.Result;
            
            if (request.WhatSearchTerm != null && Regex.IsMatch(request.WhatSearchTerm, @"^VAC\d{10}$"))
            {
                var vacancyReferenceResult = await _findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(new GetVacancyRequest(request.WhatSearchTerm));

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
                        VacancyReference = request.WhatSearchTerm
                    };
                }
            }


            var categories = routes.Routes.Where(route => request.SelectedRouteIds != null && request.SelectedRouteIds.Contains(route.Id.ToString()))
                .Select(route => route.Name).ToList();

            var resultCountTask = _findApprenticeshipApiClient.Get<GetApprenticeshipCountResponse>(
               new GetApprenticeshipCountRequest(
                   location?.GeoPoint?.FirstOrDefault(),
                   location?.GeoPoint?.LastOrDefault(),
                   request.Distance,
                   categories,
                   request.WhatSearchTerm
               ));

            var vacancyResultTask = _findApprenticeshipApiClient.Get<GetVacanciesResponse>(
                new GetVacanciesRequest(
                    location?.GeoPoint?.FirstOrDefault(),
                    location?.GeoPoint?.LastOrDefault(),
                    request.Distance,
                    request.Sort,
                    request.WhatSearchTerm,
                    request.PageNumber,
                    request.PageSize,
                    categories));

            await Task.WhenAll(resultCountTask, vacancyResultTask);

            var result = resultCountTask.Result;
            var vacancyResult = vacancyResultTask.Result;

            var totalPages = (int)Math.Ceiling((double)result.TotalVacancies / request.PageSize);

            return new SearchApprenticeshipsResult
            {
                TotalApprenticeshipCount = result.TotalVacancies,
                LocationItem = location,
                Routes = routes.Routes.ToList(),
                Vacancies = vacancyResult.ApprenticeshipVacancies.ToList(),
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = totalPages
            };
        }
    }
}