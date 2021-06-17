using MediatR;
using SFA.DAS.Encoding;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetAllPledges
{
    public class GetPledgesHandler : IRequestHandler<GetPledgesQuery, GetPledgesResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IEncodingService _encodingService;

        public GetPledgesHandler(IEncodingService encodingService, ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _encodingService = encodingService;
        }

        public async Task<GetPledgesResult> Handle(GetPledgesQuery request, CancellationToken cancellationToken)
        {
            var result = await _levyTransferMatchingService.GetAllPledges();

            foreach (var pledge in result)
            {
                pledge.EncodedPledgeId = _encodingService.Encode(pledge.Id.Value, EncodingType.PledgeId);
                pledge.EncodedAccountId = _encodingService.Encode(pledge.AccountId, EncodingType.AccountId);
            }

            return new GetPledgesResult(result);
        }
    }
}
