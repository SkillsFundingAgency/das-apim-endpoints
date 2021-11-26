using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationApproved
{
    public class GetApplicationApprovedQueryHandler : IRequestHandler<GetApplicationApprovedQuery, GetApplicationApprovedQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetApplicationApprovedQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetApplicationApprovedQueryResult> Handle(GetApplicationApprovedQuery request, CancellationToken cancellationToken)
        {
            var response = await _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.PledgeId, request.ApplicationId));

            return new GetApplicationApprovedQueryResult
            {
                EmployerAccountName = response.EmployerAccountName,
                AutomaticApproval = response.AutomaticApproval
            };
        }
    }
}
