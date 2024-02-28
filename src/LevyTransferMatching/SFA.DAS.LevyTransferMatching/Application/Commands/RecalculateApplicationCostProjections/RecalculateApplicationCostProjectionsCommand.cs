using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RecalculateApplicationCostProjections
{
    public class RecalculateApplicationCostProjectionsCommand : IRequest<Unit>
    {
    }

    public class RecalculateApplicationCostProjectionsCommandHandler : IRequestHandler<RecalculateApplicationCostProjectionsCommand, Unit>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<RecalculateApplicationCostProjectionsCommandHandler> _logger;

        public RecalculateApplicationCostProjectionsCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<RecalculateApplicationCostProjectionsCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task<Unit> Handle(RecalculateApplicationCostProjectionsCommand request, CancellationToken cancellationToken)
        {
            var getApplicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest());

            foreach (var applicationId in getApplicationsResponse.Applications.Select(a => a.Id))
            {
                _logger.LogInformation($"Recalculating cost projection for application {applicationId}");

                var apiResponse = await _levyTransferMatchingService.RecalculateApplicationCostProjection(new RecalculateApplicationCostProjectionRequest
                    {ApplicationId = applicationId });

                if (!apiResponse.StatusCode.IsSuccess())
                {
                    _logger.LogError($"Error recalculating cost projection for application {applicationId}");
                }
            }

            return Unit.Value;
        }
    }
}
