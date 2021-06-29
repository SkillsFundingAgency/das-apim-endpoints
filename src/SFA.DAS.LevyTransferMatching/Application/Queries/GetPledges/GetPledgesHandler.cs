using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges
{
    public class GetPledgesHandler : IRequestHandler<GetPledgesQuery, GetPledgesResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetPledgesHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetPledgesResult> Handle(GetPledgesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Pledge> pledges = null;

            if (request.PledgeId.HasValue)
            {
                Pledge pledge = await _levyTransferMatchingService.GetPledge(request.PledgeId.Value);

                if (pledge != null)
                {
                    pledges = new Pledge[]
                    {
                        pledge,
                    };
                }
                else
                {
                    pledges = Array.Empty<Pledge>();
                }
            }
            else
            {
                pledges = await _levyTransferMatchingService.GetPledges();
            }

            return new GetPledgesResult(pledges);
        }
    }
}