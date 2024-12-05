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

        private List<GetApplicationsResponse.Application> FilterApplications(GetApplicationsResponse getApplicationsResponse)
        {
            if (getApplicationsResponse == null)
            {
                _logger.LogInformation("GetApplicationsForAutomaticRejectionQueryHandler getApplicationsResponse == null");

                return new List<GetApplicationsResponse.Application>();
            }

            var initialCount = getApplicationsResponse.Applications.Count();
            _logger.LogInformation("GetApplicationsForAutomaticRejectionQueryHandler >FilterApplications: Initial count :{initialCount}", initialCount);


            var notApplicableCount = getApplicationsResponse.Applications
                .Count(x => x.PledgeAutomaticApprovalOption == AutomaticApprovalOption.NotApplicable && x.CreatedOn < ThreeMonthsAgo);
            _logger.LogInformation("GetApplicationsForAutomaticRejectionQueryHandler:  NotApplicable option: {notApplicableCount}", notApplicableCount);

            var applicableCount = getApplicationsResponse.Applications
        .Count(x => x.PledgeAutomaticApprovalOption != AutomaticApprovalOption.NotApplicable && x.MatchPercentage < 100 && x.CreatedOn < ThreeMonthsAgo);
            _logger.LogInformation("FilterApplications: Count of APPLICAABLE applications: {otherCount}", applicableCount);


            var filteredApplications = new List<GetApplicationsResponse.Application>();

            foreach (var application in getApplicationsResponse.Applications)
            {
                if ((application.PledgeAutomaticApprovalOption == AutomaticApprovalOption.NotApplicable && application.CreatedOn < ThreeMonthsAgo) ||
                    (application.PledgeAutomaticApprovalOption != AutomaticApprovalOption.NotApplicable && application.MatchPercentage < 100 && application.CreatedOn < ThreeMonthsAgo))
                {
                    _logger.LogInformation("GetApplicationsForAutomaticRejectionQueryHandler  adding application: Id = {filteredCount}", application.Id);

                    filteredApplications.Add(application);
                }
            }

            _logger.LogInformation("GetApplicationsForAutomaticRejectionQueryHandler> FilterApplications: Final count of filtered applications: {filteredCount}", filteredApplications.Count);

            return filteredApplications;
        }

    }
}
