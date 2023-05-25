using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticePortal.Application.Commands.ApprenticeAccounts
{
    public class ApprenticePatchCommandHandler : IRequestHandler<ApprenticePatchCommand, Unit>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _client;
        private readonly ILogger<ApprenticePatchCommandHandler> _logger;

        public ApprenticePatchCommandHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> client, ILogger<ApprenticePatchCommandHandler> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<Unit> Handle(ApprenticePatchCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[ApprenticeUpdateCommandHandler] request {@request}", request);

            var patchApprenticeRequest = new PatchApprentice
            {
                ApprenticeId = request.ApprenticeId,
                Patch = request.Patch
            };

            await _client.Patch(new PatchApprenticeRequest { Data = patchApprenticeRequest });

            return Unit.Value;
        }
    }
}