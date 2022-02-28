using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.BackfillApplicationMatchingCriteria
{
    public class BackfillApplicationMatchingCriteriaCommand : IRequest
    {
    }

    public class BackfillApplicationMatchingCriteriaCommandHandler : IRequestHandler<BackfillApplicationMatchingCriteriaCommand>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<BackfillApplicationMatchingCriteriaCommandHandler> _logger;

        public BackfillApplicationMatchingCriteriaCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<BackfillApplicationMatchingCriteriaCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task<Unit> Handle(BackfillApplicationMatchingCriteriaCommand request, CancellationToken cancellationToken)
        {
            var getApplicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest());

            foreach (var application in getApplicationsResponse.Applications.Where(x => x.MatchPercentage == 255))
            {
                _logger.LogInformation($"Backfilling matching criteria for application {application.Id}");

                var apiResponse = await _levyTransferMatchingService.GenerateMatchingCriteria(new GenerateMatchingCriteriaRequest
                    {ApplicationId = application.Id});

                if (!apiResponse.StatusCode.IsSuccess())
                {
                    _logger.LogError($"Error backfilling matching criteria for application {application.Id}");
                }
            }

            return Unit.Value;
        }
    }
}
