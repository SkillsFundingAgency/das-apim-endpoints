using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetLevels
{
    public class GetLevelsQueryHandler : IRequestHandler<GetLevelsQuery, GetLevelsQueryResult>
    {
        private readonly IReferenceDataService _referenceDataService;
        
        public GetLevelsQueryHandler(IReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        public async Task<GetLevelsQueryResult> Handle(GetLevelsQuery request, CancellationToken cancellationToken)
        {
            return new GetLevelsQueryResult
            {
                ReferenceDataItems = await _referenceDataService.GetLevels()
            };
        }
    }
}
