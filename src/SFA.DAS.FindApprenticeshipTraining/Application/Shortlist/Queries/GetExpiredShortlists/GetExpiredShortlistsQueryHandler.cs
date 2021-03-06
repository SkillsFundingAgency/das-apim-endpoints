﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetExpiredShortlists
{
    public class GetExpiredShortlistsQueryHandler : IRequestHandler<GetExpiredShortlistsQuery, GetExpiredShortlistsQueryResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;

        public GetExpiredShortlistsQueryHandler (ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
        }
        public async Task<GetExpiredShortlistsQueryResult> Handle(GetExpiredShortlistsQuery request, CancellationToken cancellationToken)
        {
            var apiResult =
                await _courseDeliveryApiClient.Get<GetExpiredShortlistsResponse>(
                    new GetExpiredShortlistsRequest(request.ExpiryInDays));

            return new GetExpiredShortlistsQueryResult
            {
                UserIds = apiResult.UserIds
            };
        }
    }
}