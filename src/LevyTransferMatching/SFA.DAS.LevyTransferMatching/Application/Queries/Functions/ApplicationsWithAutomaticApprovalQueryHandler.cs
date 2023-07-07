using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.LevyTransferMatching.Models.Constants;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Functions
{
    public  class ApplicationsWithAutomaticApprovalQueryHandler : IRequestHandler<ApplicationsWithAutomaticApprovalQuery, ApplicationsWithAutomaticApprovalQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public ApplicationsWithAutomaticApprovalQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<ApplicationsWithAutomaticApprovalQueryResult> Handle(ApplicationsWithAutomaticApprovalQuery request, CancellationToken cancellationToken)
        {   
            var getApplicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest { 
                ApplicationStatusFilter = PledgeStatus.Pending,
                SortOrder = "ApplicationDate",
                SortDirection = "Ascending"
            });

            DateTime sixWeeksAgo = DateTime.UtcNow.AddDays(-42);

            var applications = new List<GetApplicationsResponse.Application>();
            if (getApplicationsResponse != null)
            {
                applications = getApplicationsResponse.Applications
                .Where(x => x.MatchPercentage == 100 &&
                            x.AutoApproveFullMatches.HasValue &&
                           (x.AutoApproveFullMatches == true) ||
                           (x.AutoApproveFullMatches == false && x.CreatedOn <= sixWeeksAgo)
                            && (request.PledgeId.HasValue && x.PledgeId == request.PledgeId)
                           )
                    .OrderBy(x => x.PledgeId)
                    .ThenBy(x => x.CreatedOn)
                .ToList();
            }            

            var autoApprovals = GetAutoApplicationDataFromApplicationsResponse(applications);

            return new ApplicationsWithAutomaticApprovalQueryResult
            {
                Applications = autoApprovals
            };
        }

        public List<ApplicationsWithAutomaticApprovalQueryResult.Application> GetAutoApplicationDataFromApplicationsResponse(List<GetApplicationsResponse.Application> applications)
        {
            var autoApprovals = new List<ApplicationsWithAutomaticApprovalQueryResult.Application>();
            if (applications == null || applications.Count == 0)
            {
                return autoApprovals;
            }

            var pledgeId = applications[0].PledgeId;
            var remainingPledge = applications[0].PledgeRemainingAmount;

            foreach (var app in applications)
            {
                if (app.PledgeId != pledgeId)
                {
                    remainingPledge = app.PledgeRemainingAmount;
                    pledgeId = app.PledgeId;
                }

                remainingPledge -= app.TotalAmount;
                if (remainingPledge >= 0)
                {
                    autoApprovals.Add(app);
                }
            }
            return autoApprovals;
        }
    }
}
