using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ApproveAutomaticApplication
{
    public class ApproveAutomaticApplicationCommandHandler : IRequestHandler<ApproveAutomaticApplicationCommand>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<ApproveAutomaticApplicationCommandHandler> _logger;

        public ApproveAutomaticApplicationCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<ApproveAutomaticApplicationCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task<Unit> Handle(ApproveAutomaticApplicationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Approving Automatic Application {request.ApplicationId} for Pledge {request.PledgeId}");

            var apiRequestData = new ApproveApplicationRequestData
            {
                UserId = string.Empty, 
                UserDisplayName = string.Empty,
                AutomaticApproval = true 
            };

            var apiRequest = new ApproveApplicationRequest(request.PledgeId, request.ApplicationId, apiRequestData);

            await _levyTransferMatchingService.ApproveApplication(apiRequest);

            return Unit.Value;
        }
    }
}
