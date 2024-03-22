using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using AutomaticApprovalOption = SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching.AutomaticApprovalOption;

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
                ApplicationStatusFilter = ApplicationStatus.Pending,
                SortOrder = ApplicationSortColumn.ApplicationDate,
                SortDirection = SortOrder.Ascending
            });

            var sixWeeksAgo = DateTime.UtcNow.AddDays(-42);

            var applications = new List<GetApplicationsResponse.Application>();
            if (getApplicationsResponse != null)
            {
                applications = getApplicationsResponse.Applications
                .Where(x => x.MatchPercentage == 100
                    && (x.PledgeAutomaticApprovalOption == AutomaticApprovalOption.ImmediateAutoApproval
                        || (x.PledgeAutomaticApprovalOption == AutomaticApprovalOption.DelayedAutoApproval && x.CreatedOn <= sixWeeksAgo))
                    && (!request.PledgeId.HasValue || x.PledgeId == request.PledgeId))
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

            var pledgeId = applications.First().PledgeId;
            var remainingPledge = applications.First().PledgeRemainingAmount;

            foreach (var app in applications)
            {
                if (app.PledgeId != pledgeId)
                {
                    remainingPledge = app.PledgeRemainingAmount;
                    pledgeId = app.PledgeId;
                }

                remainingPledge -= app.Amount;
                if (remainingPledge >= 0)
                {
                    autoApprovals.Add(app);
                }
            }
            return autoApprovals;
        }
    }
}
