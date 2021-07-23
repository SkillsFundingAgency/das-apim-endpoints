using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication
{
    public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, CreateApplicationCommandResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IAccountsService _accountsService;
        private readonly ILogger<CreateApplicationCommandHandler> _logger;

        public CreateApplicationCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, IAccountsService accountsService, ILogger<CreateApplicationCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _accountsService = accountsService;
            _logger = logger;
        }

        public async Task<CreateApplicationCommandResult> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Creating Application to Pledge {request.PledgeId} for Account {request.EmployerAccountId}");

            var account = await _levyTransferMatchingService.GetAccount(new GetAccountRequest(request.EmployerAccountId));

            if (account == null)
            {
                _logger.LogInformation($"Account {request.EmployerAccountId} does not exist - creating");
                await CreateAccount(request);
            }

            var createApplicationRequest = new CreateApplicationRequest(request.PledgeId,
                request.EmployerAccountId,
                request.Details,
                request.StandardId,
                request.NumberOfApprentices,
                request.StartDate,
                request.HasTrainingProvider,
                request.Sectors,
                request.Postcode,
                request.FirstName,
                request.LastName,
                request.EmailAddresses,
                request.BusinessWebsite);

            var result = await _levyTransferMatchingService.CreateApplication(createApplicationRequest);

            return new CreateApplicationCommandResult
            {
                ApplicationId = result.ApplicationId
            };
        }

        private async Task CreateAccount(CreateApplicationCommand request)
        {
            var accountData = await _accountsService.GetAccount(request.EncodedAccountId);

            await _levyTransferMatchingService.CreateAccount(new CreateAccountRequest(request.EmployerAccountId,
                accountData.DasAccountName));
        }
    }
}