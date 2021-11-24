using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetWithdrawn
{
    public class GetWithdrawnQueryHandler : IRequestHandler<GetWithdrawnQuery, GetWithdrawnQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetWithdrawnQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetWithdrawnQueryResult> Handle(GetWithdrawnQuery request, CancellationToken cancellationToken)
        {
            var application = await _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.ApplicationId));

            if (application == null)
            {
                return null;
            }

            return new GetWithdrawnQueryResult()
            {
                EmployerAccountName = application.EmployerAccountName,
                OpportunityId = application.PledgeId,
            };
        }
    }
}