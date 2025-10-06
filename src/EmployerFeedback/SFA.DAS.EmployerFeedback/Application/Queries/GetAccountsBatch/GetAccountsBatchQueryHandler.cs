using MediatR;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetAccountsBatch
{
    public class GetAccountsBatchQueryHandler : IRequestHandler<GetAccountsBatchQuery, GetAccountsBatchResult>
    {
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _apiClient;

        public GetAccountsBatchQueryHandler(IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetAccountsBatchResult> Handle(GetAccountsBatchQuery request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.GetWithResponseCode<GetAccountsBatchResponse>(new GetAccountsBatchRequest(request.BatchSize));
            response.EnsureSuccessStatusCode();

            return new GetAccountsBatchResult
            {
                AccountIds = response.Body?.AccountIds
            };
        }
    }
}