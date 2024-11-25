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
        private readonly ILogger<GetApplicationsForAutomaticRejectionQueryHandler> _logger;
        private static readonly DateTime ThreeMonthsAgo = DateTime.UtcNow.AddMonths(-3);

        public GetApplicationsForAutomaticRejectionQueryHandler(ILevyTransferMatchingService levyTransferMatchingService
            , ILogger<GetApplicationsForAutomaticRejectionQueryHandler> logger)
        {
            _logger = logger;
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetApplicationsForAutomaticRejectionQueryResult> Handle(GetApplicationsForAutomaticRejectionQuery request, CancellationToken cancellationToken)
        {   
            var getApplicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest {
                ApplicationStatusFilter = ApplicationStatus.Pending,
                SortOrder = ApplicationSortColumn.ApplicationDate,
                SortDirection = SortOrder.Ascending
            });
            _logger.LogInformation("GetApplicationsForAutomaticRejectionQueryHandler returns {count} pending applications from inner api", getApplicationsResponse?.Applications.Count());
             var applications = FilterApplications(getApplicationsResponse);
            _logger.LogInformation("GetApplicationsForAutomaticRejectionQueryHandler filter returns {count_}", applications.Count);

            var result = applications.Select(application => GetApplicationsForAutomaticRejectionQueryResult.Application.BuildApplication(application)).ToList();

            return new GetApplicationsForAutomaticRejectionQueryResult
            {
                Applications = result
            };
        }

        private static List<GetApplicationsResponse.Application> FilterApplications(GetApplicationsResponse getApplicationsResponse)
        {
            if (getApplicationsResponse == null)
            {
                return new List<GetApplicationsResponse.Application>();
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
