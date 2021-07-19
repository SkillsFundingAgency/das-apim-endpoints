using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetSector
{
    public class GetSectorQueryHandler : IRequestHandler<GetSectorQuery, GetSectorQueryResult>
    {
        private readonly IReferenceDataService _referenceDataService;

        public GetSectorQueryHandler(IReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        public async Task<GetSectorQueryResult> Handle(GetSectorQuery request, CancellationToken cancellationToken)
        {
            return new GetSectorQueryResult
            {
                Sectors = await _referenceDataService.GetSectors()
            };
        }
    }
}