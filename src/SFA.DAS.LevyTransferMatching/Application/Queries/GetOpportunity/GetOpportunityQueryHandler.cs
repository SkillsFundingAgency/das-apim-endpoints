using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetOpportunity
{
    public class GetOpportunityQueryHandler : IRequestHandler<GetOpportunityQuery, GetOpportunityResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetOpportunityQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetOpportunityResult> Handle(GetOpportunityQuery request, CancellationToken cancellationToken)
        {
            var opportunity = await _levyTransferMatchingService.GetPledge(request.OpportunityId);

            if (opportunity != null)
            {
                return new GetOpportunityResult()
                {
                     AccountId = opportunity.AccountId,
                     Amount = opportunity.Amount,
                     CreatedOn = opportunity.CreatedOn,
                     DasAccountName = opportunity.DasAccountName,
                     Id = opportunity.Id,
                     IsNamePublic = opportunity.IsNamePublic,
                     JobRoles = opportunity.JobRoles,
                     Levels = opportunity.Levels,
                     Sectors = opportunity.Sectors,
                };
            }
            else
            {
                return null;
            }
        }
    }
}