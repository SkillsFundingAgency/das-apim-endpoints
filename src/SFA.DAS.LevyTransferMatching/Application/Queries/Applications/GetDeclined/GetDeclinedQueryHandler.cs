using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetDeclined
{
    public class GetDeclinedQueryHandler : IRequestHandler<GetDeclinedQuery, GetDeclinedResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetDeclinedQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetDeclinedResult> Handle(GetDeclinedQuery request, CancellationToken cancellationToken)
        {
            var application = await _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.ApplicationId));

            if (application == null)
            {
                return null;
            }

            var pledge = await _levyTransferMatchingService.GetPledge(application.PledgeId);

            return new GetDeclinedResult()
            {
                EmployerAccountName = pledge.DasAccountName,
                OpportunityId = application.PledgeId,
            };
        }
    }
}