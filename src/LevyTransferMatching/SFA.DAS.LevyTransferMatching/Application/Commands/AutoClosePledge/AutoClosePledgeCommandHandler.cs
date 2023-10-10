using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using static SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests.ClosePledgeRequest;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.AutoClosePledge
{
    public class AutoClosePledgeCommandHandler : IRequestHandler<AutoClosePledgeCommand, AutoClosePledgeCommandResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<AutoClosePledgeCommandHandler> _logger;

        public AutoClosePledgeCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<AutoClosePledgeCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task<AutoClosePledgeCommandResult> Handle(AutoClosePledgeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Checking if Pledge {request.PledgeId} is to be Auto-Closed");

            var getApplicationTask = _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.ApplicationId));
            var getPledgeTask = _levyTransferMatchingService.GetPledge(request.PledgeId);

            await Task.WhenAll(getApplicationTask, getPledgeTask);

            var application = getApplicationTask.Result;
            var pledge = getPledgeTask.Result;

            if (!IsPledgeToBeClosedAfterApproval(pledge.RemainingAmount, application.Amount))
            {
                return new AutoClosePledgeCommandResult { PledgeClosed = false};
            }
            var apiRequestData = new ClosePledgeRequestData
            {
                UserDisplayName = "",
                UserId = ""
            };

            await _levyTransferMatchingService.ClosePledge(new ClosePledgeRequest(request.PledgeId, apiRequestData));

            return new AutoClosePledgeCommandResult { PledgeClosed = true }; 
        }
        
        private bool IsPledgeToBeClosedAfterApproval(int remainingPledgeBalance, int cost)
        {
            var pledgeBalancePostApproval = remainingPledgeBalance - cost;
            return pledgeBalancePostApproval >= 0 && pledgeBalancePostApproval < 2000;
        }
    }
}