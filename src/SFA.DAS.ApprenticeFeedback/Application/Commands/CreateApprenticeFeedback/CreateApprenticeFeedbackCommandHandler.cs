using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprenticeFeedback
{
    public class CreateApprenticeFeedbackCommandHandler : IRequestHandler<CreateApprenticeFeedbackCommand, CreateApprenticeFeedbackResponse>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _feedbackApiClient;

        public CreateApprenticeFeedbackCommandHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> feedbackApiClient)
        {
            _feedbackApiClient = feedbackApiClient;
        }

        public async Task<CreateApprenticeFeedbackResponse> Handle(CreateApprenticeFeedbackCommand command, CancellationToken cancellationToken)
        {
            var request = new CreateApprenticeFeedbackRequest(new CreateApprenticeFeedbackData
            {
                ApprenticeFeedbackTargetId = command.ApprenticeFeedbackTargetId,
                OverallRating = command.OverallRating,
                AllowContact = command.AllowContact,
                FeedbackAttributes = command.FeedbackAttributes.Select(s => (FeedbackAttribute)s).ToList()
            });

            var response = await _feedbackApiClient.PostWithResponseCode<CreateApprenticeFeedbackResponse>(request);

            response.EnsureSuccessStatusCode();
            return response.Body;
        }
    }
}
