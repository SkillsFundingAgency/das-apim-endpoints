using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetSectors
{
    public class GetSectorsQueryHandler : IRequestHandler<GetSectorsQuery, GetSectorsQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        public GetSectorsQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetSectorsQueryResult> Handle(GetSectorsQuery request, CancellationToken cancellationToken)
        {
            var result = await _levyTransferMatchingService.GetSectors();
            return new GetSectorsQueryResult
            {
                ReferenceDataItems = result
            };
        }
    }
}
