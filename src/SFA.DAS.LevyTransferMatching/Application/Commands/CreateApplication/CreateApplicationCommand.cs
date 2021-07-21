using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication
{
    public class CreateApplicationCommand : IRequest<CreateApplicationCommandResult>
    {
        public int PledgeId { get; set; }
        public long EmployerAccountId { get; set; }
        public string EncodedAccountId { get; set; }
    }

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
            var account = await _levyTransferMatchingService.GetAccount(new GetAccountRequest(request.EmployerAccountId));

            if (account == null)
            {
                var maAccount = await _accountsService.GetAccount(request.EncodedAccountId);

                await _levyTransferMatchingService.CreateAccount(new CreateAccountRequest(request.EmployerAccountId,
                    maAccount.DasAccountName));
            }

            var result =
                await _levyTransferMatchingService.CreateApplication(
                    new CreateApplicationRequest(request.PledgeId, request.EmployerAccountId));

            return new CreateApplicationCommandResult
            {
                ApplicationId = result.ApplicationId
            };
        }
    }

    public class CreateApplicationCommandResult
    {
        public int ApplicationId { get; set; }
    }
}
