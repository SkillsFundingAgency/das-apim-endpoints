using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplicationApprovalOptions
{
    public class GetApplicationApprovalOptionsQueryHandler : IRequestHandler<GetApplicationApprovalOptionsQuery, GetApplicationApprovalOptionsResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetApplicationApprovalOptionsQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetApplicationApprovalOptionsResult> Handle(GetApplicationApprovalOptionsQuery request, CancellationToken cancellationToken)
        {
            var application = await _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.PledgeId, request.ApplicationId));

            if (application == null)
            {
                return null;
            }

            return new GetApplicationApprovalOptionsResult()
            {
                EmployerAccountName = application.EmployerAccountName,
            };
        }
    }
}