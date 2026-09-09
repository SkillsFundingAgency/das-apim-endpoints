using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts
{
    public class ApprenticePatchCommandHandler : IRequestHandler<ApprenticePatchCommand, Unit>
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _client;
        private readonly ILogger<ApprenticePatchCommandHandler> _logger;
        private readonly IValidator<ApprenticePatchCommand> _validator;

        public ApprenticePatchCommandHandler(
            IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> client,
            ILogger<ApprenticePatchCommandHandler> logger,
            IValidator<ApprenticePatchCommand> validator)
        {
            _client = client;
            _logger = logger;
            _validator = validator;
        }

        [ExcludeFromCodeCoverage]
        public async Task<Unit> Handle(ApprenticePatchCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[ApprenticeUpdateCommandHandler] command {@command}", command);

            var validation = await _validator.ValidateAsync(command, cancellationToken);
            if (!validation.IsValid)
            {
                _logger.LogInformation("Validation failed {errors}", validation.Errors);
                throw new ValidationException(validation.Errors);
            }

            var patchApprenticeRequest = new PatchApprenticeRequest(command.ApprenticeId)
            {
                Data = command.Patch
            };

            await _client.Patch(patchApprenticeRequest);

            return Unit.Value;
        }
    }
}