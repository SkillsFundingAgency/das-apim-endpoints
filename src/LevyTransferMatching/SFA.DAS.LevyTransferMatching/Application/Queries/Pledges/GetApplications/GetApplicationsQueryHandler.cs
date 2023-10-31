using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications
{
    public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQuery, GetApplicationsQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetApplicationsQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetApplicationsQueryResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            var applicationsTask = _levyTransferMatchingService.GetApplications(new GetApplicationsRequest
            {
                PledgeId = request.PledgeId,
                SortOrder = request.SortOrder,
                SortDirection = request.SortDirection
            });

            var pledgeTask = _levyTransferMatchingService.GetPledge(request.PledgeId);

            await Task.WhenAll(applicationsTask, pledgeTask);

            var result = new List<GetApplicationsQueryResult.Application>();
            foreach (var application in applicationsTask.Result.Applications)
            {
                result.Add(GetApplicationsQueryResult.Application.BuildApplication(application, pledgeTask.Result));
            }

            return new GetApplicationsQueryResult
            {
                Applications = result,
                PledgeStatus = pledgeTask.Result.Status,
                TotalAmount = pledgeTask.Result.Amount,
                RemainingAmount = pledgeTask.Result.RemainingAmount,
                AutomaticApprovalOption = pledgeTask.Result.AutomaticApprovalOption
            };
        }

    }
}