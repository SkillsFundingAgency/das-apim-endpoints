using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Accounts;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge
{
    public class CreatePledgeHandler : IRequestHandler<CreatePledgeCommand, CreatePledgeResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public CreatePledgeHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<CreatePledgeResult> Handle(CreatePledgeCommand command, CancellationToken cancellationToken)
        {
            //await _levyTransferMatchingService.CreateAccount(new CreateAccountRequest
            //    {AccountId = command.AccountId, AccountName = command.DasAccountName});

            var pledgeId = await _levyTransferMatchingService.CreatePledge(command);

            return new CreatePledgeResult
            {
                PledgeId = pledgeId
            };
        }
    }
}