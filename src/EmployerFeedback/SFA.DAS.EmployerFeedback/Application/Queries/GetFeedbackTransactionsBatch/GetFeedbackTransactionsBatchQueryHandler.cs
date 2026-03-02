using MediatR;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionsBatch
{
    public class GetFeedbackTransactionsBatchQueryHandler : IRequestHandler<GetFeedbackTransactionsBatchQuery, GetFeedbackTransactionsBatchResult>
    {
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _apiClient;

        public GetFeedbackTransactionsBatchQueryHandler(IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetFeedbackTransactionsBatchResult> Handle(GetFeedbackTransactionsBatchQuery request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.GetWithResponseCode<GetFeedbackTransactionsBatchResponse>(new GetFeedbackTransactionsBatchRequest(request.BatchSize));
            response.EnsureSuccessStatusCode();

            return new GetFeedbackTransactionsBatchResult
            {
                FeedbackTransactions = response.Body?.FeedbackTransactions
            };
        }
    }
}