using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using SFA.DAS.Campaign.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.Standards
{
    public class GetStandardsQueryHandler : IRequestHandler<GetStandardsQuery, GetStandardsQueryResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ICourseService _courseService;

        public GetStandardsQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ICourseService courseService)
        {
            _coursesApiClient = coursesApiClient;
            _courseService = courseService;
        }
        public async Task<GetStandardsQueryResult> Handle(GetStandardsQuery request, CancellationToken cancellationToken)
       {
            var apiRequest = new GetAvailableToStartStandardsListRequest();

            if (!string.IsNullOrWhiteSpace(request.Sector))
            {
                var sector = await GetSectorId(request.Sector);

                if (sector == null)
                {
                    return new GetStandardsQueryResult();
                }

                apiRequest.RouteIds = new List<int> { sector.Value };

            }     
            
            var standardsResponse =
                await _coursesApiClient.Get<GetStandardsListResponse>(apiRequest);

            return new GetStandardsQueryResult
            {
                Standards = standardsResponse.Standards
            };
        }

        private async Task<int?> GetSectorId(string sector)
        {
            var response = await _courseService.GetRoutes();

            return response.Routes.FirstOrDefault(c => c.Name.Equals(sector, StringComparison.CurrentCultureIgnoreCase))?.Id;
        }
    }
}