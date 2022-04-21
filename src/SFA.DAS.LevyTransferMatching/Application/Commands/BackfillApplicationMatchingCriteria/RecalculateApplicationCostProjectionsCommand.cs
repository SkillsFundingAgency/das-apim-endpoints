using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.BackfillApplicationMatchingCriteria
{
    public class RecalculateApplicationCostProjectionsCommand : IRequest
    {
    }

    public class RecalculateApplicationCostProjectionsCommandHandler : IRequestHandler<RecalculateApplicationCostProjectionsCommand>
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

            foreach (var application in getApplicationsResponse.Applications)
            {
                _logger.LogInformation($"Recalculating cost projection for application {application.Id}");

                var apiResponse = await _levyTransferMatchingService.RecalculateApplicationCostProjection(new RecalculateApplicationCostProjectionRequest
                    {ApplicationId = application.Id});

                if (!apiResponse.StatusCode.IsSuccess())
                {
                    _logger.LogError($"Error recalculating cost projection for application {application.Id}");
                }
            }

            return Unit.Value;
        }
    }
}
