using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge
{
    public class CreatePledgeHandler : IRequestHandler<CreatePledgeCommand, CreatePledgeResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<CreatePledgeHandler> _logger;

        public CreatePledgeHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<CreatePledgeHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task<CreatePledgeResult> Handle(CreatePledgeCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Creating Pledge for account {command.AccountId}");

            var account = await _levyTransferMatchingService.GetAccount(new GetAccountRequest(command.AccountId));

            if (account == null)
            {
                _logger.LogInformation($"Account {command.AccountId} does not exist - creating");
                await _levyTransferMatchingService.CreateAccount(new CreateAccountRequest(command.AccountId, command.DasAccountName));
            }

            var pledgeId = await _levyTransferMatchingService.CreatePledge(command);

            return new CreatePledgeResult
            {
                PledgeId = pledgeId
            };
        }
    }
}