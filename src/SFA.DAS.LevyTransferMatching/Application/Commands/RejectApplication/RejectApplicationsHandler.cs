using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication
{
    public class RejectApplicationsHandler : IRequestHandler<RejectApplicationsCommand>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<RejectApplicationsHandler> _logger;

        public RejectApplicationsHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<RejectApplicationsHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task<Unit> Handle(RejectApplicationsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Rejecting Application(s) { string.Join(", ", request.ApplicationsToReject) } for Pledge {request.PledgeId}");

            var apiRequestData = new RejectApplicationRequestData
            {
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName
            };
            
            foreach (var applicationId in request.ApplicationsToReject)
            {
                // TODO use applicationId from the loop in the call below after decoding the string id to int
                var apiRequest = new RejectApplicationRequest(request.PledgeId, 3 /*applicationId*/, apiRequestData);

                await _levyTransferMatchingService.RejectApplication(apiRequest);
            }
            return Unit.Value;

            // Next: Currently app id is "V5gH6 - Mega corp", get first part and get int id here, or send int IDs to APIM in the first place
        }
    }
}