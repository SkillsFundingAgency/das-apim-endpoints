using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.Campaign.Configuration;
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
            var standardsTask =
                _courseService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));
            var locationTask = _locationLookupService.GetLocationInformation(request.Postcode, 0, 0);

            await Task.WhenAll(routesTask, locationTask, standardsTask);

            if (locationTask.Result == null)
            {
                return new GetAdvertsQueryResult
                {
                    Routes = routesTask.Result.Routes,
                    Vacancies = new List<GetVacanciesListItem>()
                };
            }


            var standardLarsCode = standardsTask.Result.Standards.Where(c=>c.Route.Equals(request.Route, StringComparison.CurrentCultureIgnoreCase)).Select(c=>c.LarsCode).ToList();

            var apprenticeshipVacancies = new List<GetVacanciesListItem>(); 
               
            var skip = 0;
            var take = 15;
            
            while (true)
            {
                var standards = standardLarsCode.Skip(skip).Take(take).ToList();

                if (standards.Count == 0)
                {
                    break;
                }
                
                var advertRequest = new GetVacanciesRequest(0, 20, null, null, null, 
                    standards, null,
                    locationTask.Result.GeoPoint.FirstOrDefault(), locationTask.Result.GeoPoint.LastOrDefault(),
                    request.Distance, null, null,null, "DistanceAsc");

                var adverts = await _findApprenticeshipApiClient.Get<GetVacanciesResponse>(advertRequest);
                
                apprenticeshipVacancies.AddRange(adverts.ApprenticeshipVacancies.ToList());
                skip += take;
            }

            foreach (var advert in apprenticeshipVacancies)
            {
                advert.VacancyUrl = $"{_configuration.FindAnApprenticeshipBaseUrl}/apprenticeship/reference/{advert.VacancyReference}";
            }
            
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