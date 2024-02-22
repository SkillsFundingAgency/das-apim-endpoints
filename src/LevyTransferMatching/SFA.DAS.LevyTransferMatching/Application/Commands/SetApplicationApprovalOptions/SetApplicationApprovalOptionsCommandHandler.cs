using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationApprovalOptions
{
    public class SetApplicationApprovalOptionsCommandHandler : IRequestHandler<SetApplicationApprovalOptionsCommand>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public SetApplicationApprovalOptionsCommandHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<Unit> Handle(SetApplicationApprovalOptionsCommand request, CancellationToken cancellationToken)
        {
            var apiRequestData = new ApproveApplicationRequestData
            {
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName,
                AutomaticApproval = request.AutomaticApproval
            };

            var apiRequest = new ApproveApplicationRequest(request.PledgeId, request.ApplicationId, apiRequestData);

            await _levyTransferMatchingService.ApproveApplication(apiRequest);

            return Unit.Value;
        }
    }
}
