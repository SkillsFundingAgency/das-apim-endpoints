using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledge
{
    public class GetPledgeQueryHandler : IRequestHandler<GetPledgeQuery, GetPledgeResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetPledgeQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetPledgeResult> Handle(GetPledgeQuery request, CancellationToken cancellationToken)
        {
            var pledge = await _levyTransferMatchingService.GetPledge(request.PledgeId);

            if (pledge != null)
            {
                return new GetPledgeResult()
                {
                     AccountId = pledge.AccountId,
                     Amount = pledge.Amount,
                     CreatedOn = pledge.CreatedOn,
                     DasAccountName = pledge.DasAccountName,
                     Id = pledge.Id,
                     IsNamePublic = pledge.IsNamePublic,
                     JobRoles = pledge.JobRoles,
                     Levels = pledge.Levels,
                     Sectors = pledge.Sectors,
                };
            }
            else
            {
                return null;
            }
        }
    }
}