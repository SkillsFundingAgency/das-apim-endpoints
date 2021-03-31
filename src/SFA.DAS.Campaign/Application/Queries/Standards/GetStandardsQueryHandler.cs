using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.InnerApi.Requests;
using SFA.DAS.Campaign.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.Standards
{
    public class GetStandardsQueryHandler : IRequestHandler<GetStandardsQuery, GetStandardsQueryResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ICacheStorageService _cacheStorageService;

        public GetStandardsQueryHandler (ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ICacheStorageService cacheStorageService)
        {
            _coursesApiClient = coursesApiClient;
            _cacheStorageService = cacheStorageService;
        }
        public async Task<GetStandardsQueryResult> Handle(GetStandardsQuery request, CancellationToken cancellationToken)
        {
            var sector = await GetSectorId(request.Sector);

            if (sector == null)
            {
                return new GetStandardsQueryResult();
            }
            
            var standardsResponse =
                await _coursesApiClient.Get<GetStandardsListResponse>(new GetAvailableToStartStandardsListRequest {RouteIds = new List<int> {sector.Value}});
            
            return new GetStandardsQueryResult
            {
                Standards = standardsResponse.Standards
            };
        }

        private async Task<int?> GetSectorId(string sector)
        {
            var response = await _cacheStorageService.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse));

            if (response == null)
            {
                response = await _coursesApiClient.Get<GetRoutesListResponse>(new GetRoutesListRequest());

                await _cacheStorageService.SaveToCache(nameof(GetRoutesListResponse), response, 23);
            }

            return response.Routes.FirstOrDefault(c => c.Name.Equals(sector, StringComparison.CurrentCultureIgnoreCase))?.Id;
        }
    }
}