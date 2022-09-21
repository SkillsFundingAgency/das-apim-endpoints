
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;


namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetFeedbackTransactionsToEmail
{
    public class GetFeedbackTransactionsToEmailQueryHandler : IRequestHandler<GetFeedbackTransactionsToEmailQuery, IEnumerable<GetFeedbackTransactionsToEmailResponse>>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apiClient;

        public GetFeedbackTransactionsToEmailQueryHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IEnumerable<GetFeedbackTransactionsToEmailResponse>> Handle(GetFeedbackTransactionsToEmailQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<GetFeedbackTransactionsToEmailResponse> apiResponse =
                await _apiClient.Get<IEnumerable<GetFeedbackTransactionsToEmailResponse>>(
                    new GetFeedbackTransactionsToEmailRequest(request.BatchSize));

            return apiResponse;

        }
    }
}
