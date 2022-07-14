using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetAccepted
{
    public class GetAcceptedQueryHandler : IRequestHandler<GetAcceptedQuery, GetAcceptedResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetAcceptedQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetAcceptedResult> Handle(GetAcceptedQuery request, CancellationToken cancellationToken)
        {
            var application = await _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.ApplicationId));

            if (application == null)
            {
                return null;
            }

            var pledge = await _levyTransferMatchingService.GetPledge(application.PledgeId);

            return new GetAcceptedResult()
            {
                EmployerAccountName = pledge.DasAccountName,
                OpportunityId = application.PledgeId,
            };
        }
    }
}