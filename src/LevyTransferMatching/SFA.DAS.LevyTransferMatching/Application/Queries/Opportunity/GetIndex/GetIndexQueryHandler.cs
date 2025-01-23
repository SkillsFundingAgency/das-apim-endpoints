using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Models.Constants;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetIndex;

public class GetIndexQueryHandler(ILevyTransferMatchingService levyTransferMatchingService, IReferenceDataService referenceDataService) : IRequestHandler<GetIndexQuery, GetIndexQueryResult>
{  
    public async Task<GetIndexQueryResult> Handle(GetIndexQuery request, CancellationToken cancellationToken)
    {
        var opportunitiesTask = levyTransferMatchingService.GetPledges(new GetPledgesRequest(null, request.Sectors));
        var sectorsTask = referenceDataService.GetSectors();
        var jobRolesTask = referenceDataService.GetJobRoles();
        var levelsTask = referenceDataService.GetLevels();

        await Task.WhenAll(opportunitiesTask, sectorsTask, jobRolesTask, levelsTask);

        var opportunities = opportunitiesTask.Result.Pledges.Where(p => p.Status != PledgeStatus.Closed)
            .Select(x => new GetIndexQueryResult.Opportunity
            {
                Id = x.Id,
                Amount = x.RemainingAmount,
                IsNamePublic = x.IsNamePublic,
                DasAccountName = x.DasAccountName,
                Sectors = x.Sectors,
                JobRoles = x.JobRoles,
                Levels = x.Levels,
                Locations = x.Locations?.Select(x => x.Name),
                CreatedOn = x.CreatedOn
            }).ToList();

        return new GetIndexQueryResult
        {
            Items = opportunities.Skip(request.Offset).Take(request.Limit).ToList(),
            TotalItems = opportunities.Count(),
            PageSize = request.PageSize ?? int.MaxValue,
            Page = request.Page,
            Sectors = sectorsTask.Result,
            JobRoles = jobRolesTask.Result,
            Levels = levelsTask.Result
        };
    }
}
