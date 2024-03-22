using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationApprovalOptions
{
    public class GetApplicationApprovalOptionsQueryHandler : IRequestHandler<GetApplicationApprovalOptionsQuery, GetApplicationApprovalOptionsQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetApplicationApprovalOptionsQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetApplicationApprovalOptionsQueryResult> Handle(GetApplicationApprovalOptionsQuery request, CancellationToken cancellationToken)
        {
            var application = await _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.PledgeId, request.ApplicationId));

            if (application == null)
            {
                return null;
            }

            return new GetApplicationApprovalOptionsQueryResult()
            {
                EmployerAccountName = application.EmployerAccountName,
                ApplicationStatus = application.Status
            };
        }
    }
}
