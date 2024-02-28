using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectPledgeApplications
{
    public class RejectPledgeApplicationsCommandHandler : IRequestHandler<RejectPledgeApplicationsCommand, Unit>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<RejectPledgeApplicationsCommandHandler> _logger;

        public RejectPledgeApplicationsCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<RejectPledgeApplicationsCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task<Unit> Handle(RejectPledgeApplicationsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Rejecting pending applications for pledge {request.PledgeId}");

            var pledgeApplications = await _levyTransferMatchingService.GetApplications( new GetApplicationsRequest()
            {
                PledgeId = request.PledgeId,
                ApplicationStatusFilter = ApplicationStatus.Pending
            });

            if (pledgeApplications != null && pledgeApplications.Applications != null && pledgeApplications.Applications.Any())
            {
                _logger.LogInformation($"Rejecting {pledgeApplications.Applications.Count()} pending applications for pledge {request.PledgeId}");

                foreach (var application in pledgeApplications.Applications)
                {
                    var apiRequestData = new RejectApplicationRequestData
                    {
                        UserId = string.Empty,
                        UserDisplayName = string.Empty
                    };

                    var apiRequest = new RejectApplicationRequest(request.PledgeId, application.Id, apiRequestData);

                    await _levyTransferMatchingService.RejectApplication(apiRequest);                   
                }                
            }

            return Unit.Value;            
        }
    }
}
