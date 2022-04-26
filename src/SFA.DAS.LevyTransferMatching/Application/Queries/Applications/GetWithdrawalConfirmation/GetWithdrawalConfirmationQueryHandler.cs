using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetWithdrawalConfirmation
{
    public class GetWithdrawalConfirmationQueryHandler : IRequestHandler<GetWithdrawalConfirmationQuery, GetWithdrawalConfirmationQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetWithdrawalConfirmationQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetWithdrawalConfirmationQueryResult> Handle(GetWithdrawalConfirmationQuery request, CancellationToken cancellationToken)
        {
            var result = await _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.ApplicationId));

            if (result == null)
                return null;

            return new GetWithdrawalConfirmationQueryResult
            {
                PledgeEmployerName = result.SenderEmployerAccountName,
                PledgeId = result.PledgeId
            };
        }
    }
}
