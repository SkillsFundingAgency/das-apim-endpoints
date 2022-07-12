using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ApplicationWithdrawnAfterAcceptance
{
    public class ApplicationWithdrawnAfterAcceptanceCommandHandler : IRequestHandler<ApplicationWithdrawnAfterAcceptanceCommand>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public ApplicationWithdrawnAfterAcceptanceCommandHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<Unit> Handle(ApplicationWithdrawnAfterAcceptanceCommand request, CancellationToken cancellationToken)
        {
            var apiRequest = new CreditPledgeRequest(request.PledgeId, new CreditPledgeRequest.CreditPledgeRequestData
            {
                ApplicationId = request.ApplicationId,
                Amount = request.Amount
            });

            await _levyTransferMatchingService.CreditPledge(apiRequest);

            return Unit.Value;
        }
    }
}
