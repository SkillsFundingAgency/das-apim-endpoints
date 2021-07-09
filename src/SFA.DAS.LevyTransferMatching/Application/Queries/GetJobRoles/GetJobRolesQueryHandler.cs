using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetJobRoles
{
    public class GetJobRolesQueryHandler : IRequestHandler<GetJobRolesQuery, GetJobRolesQueryResult>
    {
        private readonly IReferenceDataService _referenceDataService;

        public GetJobRolesQueryHandler(IReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        public async Task<GetJobRolesQueryResult> Handle(GetJobRolesQuery request, CancellationToken cancellationToken)
        {
            return new GetJobRolesQueryResult
            {
                ReferenceDataItems = await _referenceDataService.GetJobRoles()
            };
        }
    }
}
