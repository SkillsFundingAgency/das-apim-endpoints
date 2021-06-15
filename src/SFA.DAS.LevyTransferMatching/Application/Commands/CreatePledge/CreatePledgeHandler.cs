using MediatR;
using SFA.DAS.Encoding;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge
{
    public class CreatePledgeHandler : IRequestHandler<CreatePledgeCommand, CreatePledgeResult>
    {
        private readonly IEncodingService _encodingService;
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public CreatePledgeHandler(IEncodingService encodingService, ILevyTransferMatchingService levyTransferMatchingService)
        {
            _encodingService = encodingService;
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<CreatePledgeResult> Handle(CreatePledgeCommand command, CancellationToken cancellationToken)
        {
            command.AccountId = _encodingService.Decode(command.EncodedAccountId, EncodingType.AccountId);

            var pledgeReference = await _levyTransferMatchingService.CreatePledge(command);

            return new CreatePledgeResult()
            {
                PledgeReference = pledgeReference,
            };
        }
    }
}