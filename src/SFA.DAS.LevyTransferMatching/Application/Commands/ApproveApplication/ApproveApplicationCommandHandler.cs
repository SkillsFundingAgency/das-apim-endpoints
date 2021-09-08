using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication
{
    public class ApproveApplicationCommandHandler : IRequestHandler<ApproveApplicationCommand>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<ApproveApplicationCommandHandler> _logger;

        public ApproveApplicationCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<ApproveApplicationCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task<Unit> Handle(ApproveApplicationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Approving Application {request.ApplicationId} for Pledge {request.PledgeId}");

            var apiRequestData = new ApproveApplicationRequestData
            {
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName
            };

            var apiRequest = new ApproveApplicationRequest(request.PledgeId, request.ApplicationId, apiRequestData);

            await _levyTransferMatchingService.ApproveApplication(apiRequest);

            return Unit.Value;
        }
    }
}