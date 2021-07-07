using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetJobRoles
{
    public class GetJobRolesQueryHandler : IRequestHandler<GetJobRolesQuery, GetJobRolesQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        public GetJobRolesQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetJobRolesQueryResult> Handle(GetJobRolesQuery request, CancellationToken cancellationToken)
        {
            var result = await _levyTransferMatchingService.GetJobRoles();
            return new GetJobRolesQueryResult
            {
                ReferenceDataItems = result
            };
        }
    }
}
