using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectPledgeApplications
{
    public class RejectPledgeApplicationsCommandHandler : IRequestHandler<RejectPledgeApplicationsCommand>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<RejectPledgeApplicationsCommandHandler> _logger;
        
        public RejectPledgeApplicationsCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<RejectPledgeApplicationsCommandHandler> logger)
        {
            _logger = logger;
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<Unit> Handle(RejectPledgeApplicationsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Auto rejecting Pending Applications for Pledge {request.PledgeId}");

            var getApplicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest
            {
                PledgeId = request.PledgeId,
                ApplicationStatusFilter = ApplicationStatus.Pending,
                SortOrder = ApplicationSortColumn.ApplicationDate,
                SortDirection = SortOrder.Ascending
            });

            var apiRequestData = new RejectApplicationRequestData
            {
                UserId = "",
                UserDisplayName = ""
            };

            foreach (var app in getApplicationsResponse.Applications)
            {
                var apiRequest = new RejectApplicationRequest(request.PledgeId, app.Id, apiRequestData);
                await _levyTransferMatchingService.RejectApplication(apiRequest);
            }
            return Unit.Value;
        }
    }
}