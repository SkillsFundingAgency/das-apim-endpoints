using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetLevels
{
    public class GetLevelsQueryHandler : IRequestHandler<GetLevelsQuery, GetLevelsQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        public GetLevelsQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetLevelsQueryResult> Handle(GetLevelsQuery request, CancellationToken cancellationToken)
        {
            var result = await _levyTransferMatchingService.GetLevels();
            return new GetLevelsQueryResult
            {
                ReferenceDataItems = result
            };
        }
    }
}
