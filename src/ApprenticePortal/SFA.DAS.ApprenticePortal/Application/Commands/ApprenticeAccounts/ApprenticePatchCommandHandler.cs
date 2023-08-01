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

        public async Task<Unit> Handle(ApprenticePatchCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[ApprenticeUpdateCommandHandler] command {@command}", command);

            var patchApprenticeRequest = new PatchApprenticeRequest(command.ApprenticeId)
            {
                Data = command.Patch
            };

            await _client.Patch(patchApprenticeRequest);

            return Unit.Value;
        }
    }
}