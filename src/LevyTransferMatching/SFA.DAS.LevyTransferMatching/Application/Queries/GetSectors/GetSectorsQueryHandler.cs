using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetSectors
{
    public class GetSectorsQueryHandler : IRequestHandler<GetSectorsQuery, GetSectorsQueryResult>
    {
        private readonly IReferenceDataService _referenceDataService;

        public GetSectorsQueryHandler(IReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        public async Task<GetSectorsQueryResult> Handle(GetSectorsQuery request, CancellationToken cancellationToken)
        {
            return new GetSectorsQueryResult
            {
                ReferenceDataItems = await _referenceDataService.GetSectors()
            };
        }
    }
}
