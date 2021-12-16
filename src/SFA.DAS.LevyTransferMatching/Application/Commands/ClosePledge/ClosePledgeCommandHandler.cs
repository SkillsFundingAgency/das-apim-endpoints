using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

            var data = new ClosePledgeRequest.ClosePledgeRequestData(request.PledgeId);

            var closeRequest = new ClosePledgeRequest(request.PledgeId, data);

            var response = await _levyTransferMatchingService.ClosePledge(closeRequest);

            return new ClosePledgeCommandResult
            {
                Updated = response.Updated,
                Message = response.Message,
            };
        }
    }
}
