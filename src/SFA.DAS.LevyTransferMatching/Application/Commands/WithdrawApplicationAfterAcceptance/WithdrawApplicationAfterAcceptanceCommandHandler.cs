using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplicationAfterAcceptance
{
    public class WithdrawApplicationAfterAcceptanceCommandHandler : IRequestHandler<WithdrawApplicationAfterAcceptanceCommand, WithdrawApplicationAfterAcceptanceCommandResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public WithdrawApplicationAfterAcceptanceCommandHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<WithdrawApplicationAfterAcceptanceCommandResult> Handle(WithdrawApplicationAfterAcceptanceCommand request, CancellationToken cancellationToken)
        {
            var apiRequest = new WithdrawApplicationAfterAcceptanceRequest(request.AccountId, request.ApplicationId, new WithdrawApplicationAfterAcceptanceRequestData
            {
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName
            });

            var response = await _levyTransferMatchingService.WithdrawApplicationAfterAcceptance(apiRequest, cancellationToken);

            return new WithdrawApplicationAfterAcceptanceCommandResult
            {
                StatusCode = response.StatusCode,
                ErrorContent = response.ErrorContent
            };
        }
    }
}
