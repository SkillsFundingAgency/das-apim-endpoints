using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using Microsoft.Extensions.Logging;


namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions
{
    public  class GetApplicationsForAutomaticRejectionQueryHandler : IRequestHandler<GetApplicationsForAutomaticRejectionQuery, GetApplicationsForAutomaticRejectionQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private static readonly DateTime ThreeMonthsAgo = DateTime.UtcNow.AddMonths(-3);
        private readonly ILogger<GetApplicationsForAutomaticRejectionQueryHandler> _logger;


        public GetApplicationsForAutomaticRejectionQueryHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<GetApplicationsForAutomaticRejectionQueryHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task<GetApplicationsForAutomaticRejectionQueryResult> Handle(GetApplicationsForAutomaticRejectionQuery request, CancellationToken cancellationToken)
        {   
            var getApplicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest {
                ApplicationStatusFilter = ApplicationStatus.Pending,
                SortOrder = ApplicationSortColumn.ApplicationDate,
                SortDirection = SortOrder.Ascending
            });
           
            var applications = FilterApplications(getApplicationsResponse);

            var result = applications.Select(GetApplicationsForAutomaticRejectionQueryResult.Application.BuildApplication).ToList();

            _logger.LogInformation("GetApplicationsForAutomaticRejectionQueryHandler has returned {count} records from the inner api", result.Count);

            foreach (var app in result)
            {
                _logger.LogInformation("GetApplicationsForAutomaticRejectionQueryHandler appId {appId} , pledgeId {pledgeId}", app.Id, app.PledgeId);

            }
            return new GetApplicationsForAutomaticRejectionQueryResult
            {
                Applications = result
            };
        }

        private static List<GetApplicationsResponse.Application> FilterApplications(GetApplicationsResponse getApplicationsResponse)
        {
            if (getApplicationsResponse == null)
            {
                return [];
            }
          
            return getApplicationsResponse.Applications
                .Where(x =>
                    (x.PledgeAutomaticApprovalOption == AutomaticApprovalOption.NotApplicable && x.CreatedOn < ThreeMonthsAgo)
                    ||
                    (x.PledgeAutomaticApprovalOption != AutomaticApprovalOption.NotApplicable && x.MatchPercentage < 100 && x.CreatedOn < ThreeMonthsAgo)
                )
                .ToList();
        }

    }
}
