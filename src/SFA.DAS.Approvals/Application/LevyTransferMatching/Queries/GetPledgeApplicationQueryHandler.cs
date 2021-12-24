using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.LevyTransferMatching.Queries
{
    public class GetPledgeApplicationQueryHandler : IRequestHandler<GetPledgeApplicationQuery, GetPledgeApplicationResult>
    {
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _apiClient;

        public GetPledgeApplicationQueryHandler(ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetPledgeApplicationResult> Handle(GetPledgeApplicationQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetPledgeApplicationResponse>(new GetPledgeApplicationRequest(request.PledgeApplicationId));

            if (result == null) 
                return null;

            return new GetPledgeApplicationResult
            {
                SenderEmployerAccountId = result.SenderEmployerAccountId,
                ReceiverEmployerAccountId = result.ReceiverEmployerAccountId,
                Status = result.Status,
                AutomaticApproval = result.AutomaticApproval,
                TotalAmount = result.TotalAmount,
                AmountUsed = result.AmountUsed,
                AmountRemaining = result.AmountRemaining,
                PledgeId = result.PledgeId
            };
        }
    }
}