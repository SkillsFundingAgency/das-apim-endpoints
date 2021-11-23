using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreditPledge
{
    public class CreditPledgeCommandHandler : IRequestHandler<CreditPledgeCommand, CreditPledgeCommandResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<CreditPledgeCommandHandler> _logger;

        public CreditPledgeCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<CreditPledgeCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task<CreditPledgeCommandResult> Handle(CreditPledgeCommand request, CancellationToken cancellationToken)
        {
            if (request.Amount <= 0)
            {
                _logger.LogInformation($"Amount is 0 for pledge {request.PledgeId} - pledge will *not* be credited");

                return new CreditPledgeCommandResult()
                {
                    CreditPledgeSkipped = true,
                };
            }

            _logger.LogInformation($"Crediting pledge {request.PledgeId}");

            var data = new CreditPledgeRequest.CreditPledgeRequestData
            {
                Amount = request.Amount,
                ApplicationId = request.ApplicationId
            };

            var debitRequest = new CreditPledgeRequest(request.PledgeId, data);

            var response = await _levyTransferMatchingService.CreditPledge(debitRequest);

            return new CreditPledgeCommandResult
            {
                ErrorContent = response.ErrorContent,
                StatusCode = response.StatusCode
            };
        }
    }
}