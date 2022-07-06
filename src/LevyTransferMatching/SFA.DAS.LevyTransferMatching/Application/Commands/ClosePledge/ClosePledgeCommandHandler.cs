using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests.ClosePledgeRequest;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge
{
    public class ClosePledgeCommandHandler : IRequestHandler<ClosePledgeCommand, ClosePledgeCommandResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<ClosePledgeCommandHandler> _logger;

        public ClosePledgeCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<ClosePledgeCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task<ClosePledgeCommandResult> Handle(ClosePledgeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Closing pledge {request.PledgeId}");

            var apiRequestData = new ClosePledgeRequestData
            {
                UserDisplayName = request.UserDisplayName,
                UserId = request.UserId
            };

            var apiRequest = new ClosePledgeRequest(request.PledgeId, apiRequestData);

            var response = await _levyTransferMatchingService.ClosePledge(apiRequest);

            return new ClosePledgeCommandResult
            {
                ErrorContent = response.ErrorContent,
                StatusCode = response.StatusCode
            };

        }
    }
}
