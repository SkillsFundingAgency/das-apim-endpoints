using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications;

public class GetApplicationsQueryHandler(ILevyTransferMatchingService levyTransferMatchingService) : IRequestHandler<GetApplicationsQuery, GetApplicationsQueryResult>
{
    public async Task<GetApplicationsQueryResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
    {
        var applicationsTask = levyTransferMatchingService.GetApplications(new GetApplicationsRequest
        {
            PledgeId = request.PledgeId,
            SortOrder = request.SortOrder,
            SortDirection = request.SortDirection
        });

        var pledgeTask = levyTransferMatchingService.GetPledge(request.PledgeId);

        await Task.WhenAll(applicationsTask, pledgeTask);

        var result = new List<PledgeApplication>();
        foreach (var application in applicationsTask.Result.Applications)
        {
            result.Add(PledgeApplication.BuildApplication(application, pledgeTask.Result));
        }

        return new GetApplicationsQueryResult
        {
            Items = result.Skip(request.Offset).Take(request.Limit).ToList(),
            TotalItems = result.Count,
            PageSize = request.PageSize ?? int.MaxValue,
            Page = request.Page,
            PledgeStatus = pledgeTask.Result.Status,
            TotalAmount = pledgeTask.Result.Amount,
            RemainingAmount = pledgeTask.Result.RemainingAmount,
            AutomaticApprovalOption = pledgeTask.Result.AutomaticApprovalOption,
            TotalPendingApplications = pledgeTask.Result.TotalPendingApplications
        };
    }
}