using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge
{
    public class UndoApplicationApprovalCommandHandler : IRequestHandler<UndoApplicationApprovalCommand, UndoApplicationApprovalResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<UndoApplicationApprovalCommandHandler> _logger;

        public UndoApplicationApprovalCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<UndoApplicationApprovalCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task<UndoApplicationApprovalResult> Handle(UndoApplicationApprovalCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Undoing application approval for pledge {request.PledgeId} application {request.ApplicationId}");

            var undoRequest = new UndoApplicationApprovalRequest(request.PledgeId, request.ApplicationId);

            var response = await _levyTransferMatchingService.UndoApplicationApproval(undoRequest);

            return new UndoApplicationApprovalResult
            {
                ErrorContent = response.ErrorContent,
                StatusCode = response.StatusCode
            };
        }
    }
}