using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetSector
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
            var sectors = await _referenceDataService.GetSectors();

            return new GetSectorQueryResult
            {
                Sectors = sectors
            };
        }
    }
}
