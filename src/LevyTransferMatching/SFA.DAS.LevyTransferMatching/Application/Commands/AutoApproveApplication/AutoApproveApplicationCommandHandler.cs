using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.AutoApproveApplication
{
    public class AutoApproveApplicationCommandHandler : IRequestHandler<AutoApproveApplicationCommand>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<AutoApproveApplicationCommandHandler> _logger;

        public AutoApproveApplicationCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<AutoApproveApplicationCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task Handle(AutoApproveApplicationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Auto Approving Application {request.ApplicationId} for Pledge {request.PledgeId}");

            var apiRequestData = new ApproveApplicationRequestData
            {
                UserId = string.Empty, 
                UserDisplayName = string.Empty,
                AutomaticApproval = true 
            };

            var apiRequest = new ApproveApplicationRequest(request.PledgeId, request.ApplicationId, apiRequestData);

            await _levyTransferMatchingService.ApproveApplication(apiRequest);
        }
    }
}
