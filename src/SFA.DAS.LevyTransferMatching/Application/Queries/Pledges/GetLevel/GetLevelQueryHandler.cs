using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetLevel
{
    public class GetLevelQueryHandler : IRequestHandler<GetLevelQuery, GetLevelQueryResult>
    {
        private readonly IReferenceDataService _referenceDataService;

        public GetLevelQueryHandler(IReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        public async Task<GetLevelQueryResult> Handle(GetLevelQuery request, CancellationToken cancellationToken)
        {
            return new GetLevelQueryResult
            {
                Levels = await _referenceDataService.GetLevels()
            };
        }
    }
}