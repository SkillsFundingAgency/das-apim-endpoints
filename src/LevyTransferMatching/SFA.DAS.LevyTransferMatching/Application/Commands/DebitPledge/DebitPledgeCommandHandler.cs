using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge
{
    public class DebitPledgeCommandHandler : IRequestHandler<DebitPledgeCommand, DebitPledgeCommandResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<DebitPledgeCommandHandler> _logger;

        public DebitPledgeCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<DebitPledgeCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task<DebitPledgeCommandResult> Handle(DebitPledgeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Debiting pledge {request.PledgeId}");

            var data = new DebitPledgeRequest.DebitPledgeRequestData
            {
                Amount = request.Amount,
                ApplicationId = request.ApplicationId
            };
            
            var debitRequest = new DebitPledgeRequest(request.PledgeId, data);

            var response = await _levyTransferMatchingService.DebitPledge(debitRequest);

            return new DebitPledgeCommandResult
            {
                ErrorContent = response.ErrorContent,
                StatusCode = response.StatusCode
            };
        }
    }
}