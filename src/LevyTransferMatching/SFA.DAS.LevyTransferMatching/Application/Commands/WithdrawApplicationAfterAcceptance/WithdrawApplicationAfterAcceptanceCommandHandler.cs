using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplicationAfterAcceptance
{
    public class WithdrawApplicationAfterAcceptanceCommandHandler : IRequestHandler<WithdrawApplicationAfterAcceptanceCommand, Unit>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public WithdrawApplicationAfterAcceptanceCommandHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<Unit> Handle(WithdrawApplicationAfterAcceptanceCommand request, CancellationToken cancellationToken)
        {
            var apiRequest = new WithdrawApplicationRequest(request.ApplicationId, request.AccountId, new WithdrawApplicationRequestData
            {
                ApplicationId = request.ApplicationId,
                AccountId = request.AccountId,
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName
            });

            await _levyTransferMatchingService.WithdrawApplication(apiRequest, cancellationToken);

            return Unit.Value;
        }
    }
}
