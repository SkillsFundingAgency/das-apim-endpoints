using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.ApprenticeFeedback.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetFeedbackTargetsForUpdate
{
    public class GetFeedbackTargetsForUpdateQueryHandler : IRequestHandler<GetFeedbackTargetsForUpdateQuery, GetFeedbackTargetsForUpdateResult>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _feedbackApiClient;

        public GetFeedbackTargetsForUpdateQueryHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> feedbackApiClient)
        {
            _feedbackApiClient = feedbackApiClient;
        }

        public async Task<GetFeedbackTargetsForUpdateResult> Handle(GetFeedbackTargetsForUpdateQuery request, CancellationToken cancellationToken)
        {
            var apiResponse =
                await _feedbackApiClient.GetAll<ApprenticeFeedbackTargetForUpdate>(
                    new GetApprenticeFeedbackTargetsForUpdateRequest() { BatchSize = request.BatchSize });

            return new GetFeedbackTargetsForUpdateResult
            {
                FeedbackTargetsForUpdate = apiResponse.Select(aft => (FeedbackTargetForUpdate)aft).ToList()
            };
        }
    }
}