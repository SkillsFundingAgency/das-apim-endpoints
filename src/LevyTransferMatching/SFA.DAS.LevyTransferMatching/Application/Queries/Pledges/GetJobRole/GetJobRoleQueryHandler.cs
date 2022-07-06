using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetJobRole
{
    public class GetJobRoleQueryHandler : IRequestHandler<GetJobRoleQuery, GetJobRoleQueryResult>
    {
        private readonly IReferenceDataService _referenceDataService;

        public GetJobRoleQueryHandler(IReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        public async Task<GetJobRoleQueryResult> Handle(GetJobRoleQuery request, CancellationToken cancellationToken)
        {
            var jobRolesTask = _referenceDataService.GetJobRoles();
            var sectorsTask = _referenceDataService.GetSectors();

            await Task.WhenAll(jobRolesTask, sectorsTask);

            return new GetJobRoleQueryResult
            {
                JobRoles = jobRolesTask.Result,
                Sectors = sectorsTask.Result
            };
        }
    }
}