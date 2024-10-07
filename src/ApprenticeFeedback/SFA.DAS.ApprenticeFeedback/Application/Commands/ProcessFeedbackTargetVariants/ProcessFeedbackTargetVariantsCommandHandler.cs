using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;


namespace SFA.DAS.ApprenticeFeedback.Application.Commands.ProcessFeedbackTargetVariants
{
    public class ProcessFeedbackTargetVariantsCommandHandler : IRequestHandler<ProcessFeedbackTargetVariantsCommand, NullResponse>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apiClient;

        public ProcessFeedbackTargetVariantsCommandHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<NullResponse> Handle(ProcessFeedbackTargetVariantsCommand command, CancellationToken cancellationToken)
        {
            var request = new ProcessFeedbackTargetVariantsRequest(
                new ProcessFeedbackTargetVariantsData(command.ClearStaging, command.MergeStaging, command.FeedbackTargetVariants));

            var response = await _apiClient.PostWithResponseCode<NullResponse>(request, false);

            response.EnsureSuccessStatusCode();
            return response.Body;
        }
    }
}
