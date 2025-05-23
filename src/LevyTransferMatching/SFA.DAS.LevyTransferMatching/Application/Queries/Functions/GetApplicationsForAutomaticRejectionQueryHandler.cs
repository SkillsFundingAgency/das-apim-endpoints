﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions
{
    public class GetApplicationsForAutomaticRejectionQueryHandler : IRequestHandler<GetApplicationsForAutomaticRejectionQuery, GetApplicationsForAutomaticRejectionQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetApplicationsForAutomaticRejectionQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetApplicationsForAutomaticRejectionQueryResult> Handle(GetApplicationsForAutomaticRejectionQuery request, CancellationToken cancellationToken)
        {
            var getApplicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest
            {
                ApplicationStatusFilter = ApplicationStatus.Pending,
                SortOrder = ApplicationSortColumn.ApplicationDate,
                SortDirection = SortOrder.Ascending
            });

            var applications = FilterApplications(getApplicationsResponse);

            var result = applications.Select(application => GetApplicationsForAutomaticRejectionQueryResult.Application.BuildApplication(application)).ToList();

            return new GetApplicationsForAutomaticRejectionQueryResult
            {
                Applications = result
            };
        }

        private static List<GetApplicationsResponse.Application> FilterApplications(GetApplicationsResponse getApplicationsResponse)
        {
            var threeMonthsAgo = DateTime.UtcNow.AddMonths(-3);

            if (getApplicationsResponse == null)
            {
                return new List<GetApplicationsResponse.Application>();
            }

            return getApplicationsResponse.Applications
                .Where(x =>
                    (x.PledgeAutomaticApprovalOption == AutomaticApprovalOption.NotApplicable && x.CreatedOn < threeMonthsAgo)
                    ||
                    (x.PledgeAutomaticApprovalOption != AutomaticApprovalOption.NotApplicable && x.MatchPercentage < 100 && x.CreatedOn < threeMonthsAgo)
                )
                .ToList();
        }

    }
}