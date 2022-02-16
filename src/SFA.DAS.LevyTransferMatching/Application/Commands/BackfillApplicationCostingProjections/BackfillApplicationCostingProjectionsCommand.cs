using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.BackfillApplicationCostingProjections
{
    public class BackfillApplicationCostingProjectionsCommand : IRequest
    {
    }

    public class BackfillApplicationCostingProjectionsCommandHandler : IRequestHandler<BackfillApplicationCostingProjectionsCommand>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<BackfillApplicationCostingProjectionsCommandHandler> _logger;

        public BackfillApplicationCostingProjectionsCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<BackfillApplicationCostingProjectionsCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task<Unit> Handle(BackfillApplicationCostingProjectionsCommand request, CancellationToken cancellationToken)
        {
            var getApplicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest());

            foreach (var application in getApplicationsResponse.Applications.Where(x => !x.CostProjections.Any()))
            {
                _logger.LogInformation($"Backfilling cost projections for application {application.Id}");

                var apiResponse = await _levyTransferMatchingService.GenerateCostProjection(new GenerateCostProjectionRequest
                    {ApplicationId = application.Id});

                if (!apiResponse.StatusCode.IsSuccess())
                {
                    _logger.LogError($"Error backfilling cost projections for application {application.Id}");
                }
            }

            return Unit.Value;
        }
    }
}
