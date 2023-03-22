using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticePortal.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticePortal.Application.Commands.ApprenticeUpdate
{
    public class ApprenticeUpdateCommandHandler : IRequestHandler<ApprenticeUpdateCommand, Unit>
    {
        private readonly IApprenticePortalService _apprenticePortalService;
        private readonly ILogger<ApprenticeUpdateCommandHandler> _logger;

        public ApprenticeUpdateCommandHandler(IApprenticePortalService apprenticePortalService, ILogger<ApprenticeUpdateCommandHandler> logger)
        {
            _apprenticePortalService = apprenticePortalService;
            _logger = logger;
        }

        public async Task<Unit> Handle(ApprenticeUpdateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[ApprenticeUpdateCommandHandler] request {@request}", request);

            var updateApprenticeRequest = new UpdateApprenticeRequest
            {
                ApprenticeId = request.ApprenticeId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
            };

            await _apprenticePortalService.UpdateApprentice(updateApprenticeRequest);

            return Unit.Value;
        }
    }
}