using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication
{
    public class RejectApplicationCommandHandler : IRequestHandler<RejectApplicationCommand>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<RejectApplicationCommandHandler> _logger;

        public RejectApplicationCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<RejectApplicationCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task<Unit> Handle(RejectApplicationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Auto Rejecting Application over 3 months old {request.ApplicationId} for Pledge {request.PledgeId}");

            var apiRequestData = new RejectApplicationRequestData
            {
                UserId = string.Empty, 
                UserDisplayName = string.Empty
            };

            var apiRequest = new RejectApplicationRequest(request.PledgeId, request.ApplicationId, apiRequestData);

            await _levyTransferMatchingService.RejectApplication(apiRequest);

            return Unit.Value;
        }
    }
}
