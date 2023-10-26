using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Types;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationOutcome
{
    public class SetApplicationOutcomeCommandHandler : IRequestHandler<SetApplicationOutcomeCommand>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<SetApplicationOutcomeCommandHandler> _logger;

        public SetApplicationOutcomeCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<SetApplicationOutcomeCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task Handle(SetApplicationOutcomeCommand request, CancellationToken cancellationToken)
        {
            if (request.Outcome == ApplicationOutcome.Approve)
            {
                _logger.LogInformation($"Approving Application {request.ApplicationId} for Pledge {request.PledgeId}");

                var apiRequestData = new ApproveApplicationRequestData
                {
                    UserId = request.UserId,
                    UserDisplayName = request.UserDisplayName
                };

                var apiRequest = new ApproveApplicationRequest(request.PledgeId, request.ApplicationId, apiRequestData);

                await _levyTransferMatchingService.ApproveApplication(apiRequest);
            }
            else if(request.Outcome == ApplicationOutcome.Reject)
            {
                _logger.LogInformation($"Rejecting Application {request.ApplicationId} for Pledge {request.PledgeId}");

                var apiRequestData = new RejectApplicationRequestData
                {
                    UserId = request.UserId,
                    UserDisplayName = request.UserDisplayName
                };

                var apiRequest = new RejectApplicationRequest(request.PledgeId, request.ApplicationId, apiRequestData);

                await _levyTransferMatchingService.RejectApplication(apiRequest);
            }
        }
    }
}