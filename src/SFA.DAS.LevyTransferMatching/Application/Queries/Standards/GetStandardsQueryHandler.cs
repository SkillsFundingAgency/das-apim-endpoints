﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Standards
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
            var standardsResponse = await _coursesApiClient.Get<GetStandardsListResponse>(new GetActiveStandardsListRequest());

            return new GetStandardsQueryResult
            {
                Standards = standardsResponse.Standards
            };
        }        
    }
}