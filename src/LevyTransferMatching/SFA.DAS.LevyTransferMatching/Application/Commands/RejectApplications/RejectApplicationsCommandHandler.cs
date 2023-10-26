using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplications
{
    public class RejectApplicationsCommandHandler : IRequestHandler<RejectApplicationsCommand>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<RejectApplicationsCommandHandler> _logger;

        public RejectApplicationsCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<RejectApplicationsCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task Handle(RejectApplicationsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Rejecting Application(s) { string.Join(", ", request.ApplicationsToReject) } for Pledge {request.PledgeId}");

            var apiRequestData = new RejectApplicationRequestData
            {
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName
            };
            
            foreach (var applicationId in request.ApplicationsToReject)
            {
                var apiRequest = new RejectApplicationRequest(request.PledgeId, applicationId, apiRequestData);
                await _levyTransferMatchingService.RejectApplication(apiRequest);
            }
        }
    }
}