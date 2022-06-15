using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunities.GetConfirmation
{
    public class GetConfirmationQueryHandler : IRequestHandler<GetConfirmationQuery, GetConfirmationQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetConfirmationQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }
        
        public async Task<GetConfirmationQueryResult> Handle(GetConfirmationQuery request, CancellationToken cancellationToken)
        {
            var pledge = await _levyTransferMatchingService.GetPledge(request.OpportunityId);

            return new GetConfirmationQueryResult
            {
                AccountName = pledge.DasAccountName,
                IsNamePublic = pledge.IsNamePublic
            };
        }
    }
}