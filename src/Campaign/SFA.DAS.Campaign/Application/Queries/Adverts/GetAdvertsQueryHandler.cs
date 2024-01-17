using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.InnerApi.Requests;
using SFA.DAS.Campaign.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.Adverts
{
    public class GetAdvertsQueryHandler : IRequestHandler<GetAdvertsQuery, GetAdvertsQueryResult>
    {
        private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;
        private readonly ICourseService _courseService;
        private readonly ILocationLookupService _locationLookupService;
        private readonly CampaignConfiguration _configuration;

        public GetAdvertsQueryHandler (
            IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,  
            ICourseService courseService, 
            ILocationLookupService locationLookupService,
            IOptions<CampaignConfiguration> configuration)
        {
            _findApprenticeshipApiClient = findApprenticeshipApiClient;
            _courseService = courseService;
            _locationLookupService = locationLookupService;
            _configuration = configuration.Value;
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

            
            var vacancyResult = await _findApprenticeshipApiClient.Get<GetVacanciesResponse>(
                new GetVacanciesRequest(
                    locationTask.Result.GeoPoint.FirstOrDefault(),
                    locationTask.Result.GeoPoint.LastOrDefault(),
                    request.Distance,
                    0,
                    20,
                    request.Route));


            var apprenticeshipVacancies = vacancyResult.ApprenticeshipVacancies.Select(advert => new GetVacanciesListItem
            {
                VacancyReference = advert.VacancyReference,
                Distance = advert.Distance,
                VacancyUrl = $"{_configuration.FindAnApprenticeshipBaseUrl}/apprenticeship/reference/{advert.VacancyReference}"
            }).ToList();

            
            return new GetAdvertsQueryResult
            {
                Location = locationTask.Result,
                Routes = routesTask.Result.Routes,
                TotalFound = apprenticeshipVacancies.Count,
                Vacancies = apprenticeshipVacancies.OrderBy(c=>c.Distance).Take(20)
            };
        }
    }
}