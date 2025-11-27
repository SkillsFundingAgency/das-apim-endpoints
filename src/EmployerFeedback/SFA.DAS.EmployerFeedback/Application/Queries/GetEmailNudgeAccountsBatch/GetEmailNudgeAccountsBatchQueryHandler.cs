using MediatR;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetEmailNudgeAccountsBatch
{
    public class GetEmailNudgeAccountsBatchQueryHandler : IRequestHandler<GetEmailNudgeAccountsBatchQuery, GetEmailNudgeAccountsBatchResult>
    {
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _apiClient;

        public GetEmailNudgeAccountsBatchQueryHandler(IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetEmailNudgeAccountsBatchResult> Handle(GetEmailNudgeAccountsBatchQuery request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.GetWithResponseCode<GetEmailNudgeAccountsBatchResponse>(new GetEmailNudgeAccountsBatchRequest(request.BatchSize));
            response.EnsureSuccessStatusCode();

            return new GetEmailNudgeAccountsBatchResult
            {
                AccountIds = response.Body?.AccountIds
            };
        }
    }
}