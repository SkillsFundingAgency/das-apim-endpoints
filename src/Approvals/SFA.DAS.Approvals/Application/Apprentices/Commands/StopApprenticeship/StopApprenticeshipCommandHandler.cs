using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Commands.StopApprenticeship
{
    public class StopApprenticeshipCommandHandler : IRequestHandler<StopApprenticeshipCommand, Unit>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsApiClient;
        public StopApprenticeshipCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        {
            _commitmentsApiClient = commitmentsV2ApiClient;
        }

        public async Task<Unit> Handle(StopApprenticeshipCommand request, CancellationToken cancellationToken)
        {
            var body = new StopApprenticeshipRequest.Body
            {
                AccountId = request.AccountId,
                MadeRedundant = request.MadeRedundant,
                StopDate = request.StopDate,
                UserInfo = request.UserInfo
            };

            var apiRequest = new StopApprenticeshipRequest(request.ApprenticeshipId, body);

            var response = await _commitmentsApiClient.PostWithResponseCode<StopApprenticeshipRequestResponse>(apiRequest, false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
