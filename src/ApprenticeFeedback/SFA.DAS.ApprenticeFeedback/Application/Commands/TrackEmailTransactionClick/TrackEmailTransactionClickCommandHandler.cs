using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.TrackEmailTransactionClick
{
    public class TrackEmailTransactionClickCommandHandler : IRequestHandler<TrackEmailTransactionClickCommand, TrackEmailTransactionClickResponse>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apiClient;

        public TrackEmailTransactionClickCommandHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<TrackEmailTransactionClickResponse> Handle(TrackEmailTransactionClickCommand command, CancellationToken cancellationToken)
        {
            var request = new TrackEmailTransactionClickRequest(new TrackEmailTransactionClickData(
                command.FeedbackTransactionId,
                command.ApprenticeFeedbackTargetId,
                command.LinkName,
                command.LinkUrl,
                command.ClickedOn
            ));

            var response = await _apiClient.PostWithResponseCode<TrackEmailTransactionClickResponse>(request);

            response.EnsureSuccessStatusCode();
            return response.Body;
        }
    }
}
