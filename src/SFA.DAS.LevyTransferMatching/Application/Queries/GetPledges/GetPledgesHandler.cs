using MediatR;
using SFA.DAS.Encoding;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using System;
using System.Collections.Generic;
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
            IEnumerable<Pledge> pledges = null;

            var encodedId = request.EncodedId;
            if (!string.IsNullOrEmpty(encodedId))
            {
                try
                {
                    int id = (int)_encodingService.Decode(encodedId, EncodingType.PledgeId);

                    pledges = new Pledge[]
                    {
                        await _levyTransferMatchingService.GetPledge(id),
                    };
                }
                catch (IndexOutOfRangeException)
                {
                    // Appears to be thrown when an invalid encoded value is
                    // provided.
                    pledges = Array.Empty<Pledge>();
                }
            }
            else
            {
                pledges = await _levyTransferMatchingService.GetPledges();
            }

            foreach (var pledge in pledges)
            {
                pledge.EncodedPledgeId = _encodingService.Encode(pledge.Id.Value, EncodingType.PledgeId);
                pledge.EncodedAccountId = _encodingService.Encode(pledge.AccountId, EncodingType.AccountId);
            }

            return new GetPledgesResult(pledges);
        }
    }
}
