using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetCreate
{
    public class GetCreateQueryHandler : IRequestHandler<GetCreateQuery, GetCreateQueryResult>
    {
        private readonly IReferenceDataService _referenceDataService;

        public GetCreateQueryHandler(IReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        public async Task<GetCreateQueryResult> Handle(GetCreateQuery request, CancellationToken cancellationToken)
        {
            var levelsTask = _referenceDataService.GetLevels();
            var sectorsTask = _referenceDataService.GetSectors();
            var jobRolesTask = _referenceDataService.GetJobRoles();

            await Task.WhenAll(levelsTask, sectorsTask, jobRolesTask);

            return new GetCreateQueryResult{
                Levels = levelsTask.Result,
                Sectors = sectorsTask.Result,
                JobRoles = jobRolesTask.Result
            };
        }
    }
}