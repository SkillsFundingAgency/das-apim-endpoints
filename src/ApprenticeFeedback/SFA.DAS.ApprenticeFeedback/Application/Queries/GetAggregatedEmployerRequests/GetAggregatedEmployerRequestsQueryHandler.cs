using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetAggregatedEmployerRequests
{
    public class GetAggregatedEmployerRequestsQueryHandler : IRequestHandler<GetAggregatedEmployerRequestsQuery, GetAggregatedEmployerRequestsResult>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly ILogger<GetAggregatedEmployerRequestsQueryHandler> _logger;

        public GetAggregatedEmployerRequestsQueryHandler(
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            ILogger<GetAggregatedEmployerRequestsQueryHandler> logger)
        {
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _logger = logger;
        }

        public async Task<GetAggregatedEmployerRequestsResult> Handle(GetAggregatedEmployerRequestsQuery request, CancellationToken cancellationToken)
        {
            var result = await _apprenticeFeedbackApiClient.
                Get<GetAggregatedEmployerRequestsResult>(new GetAggregatedEmployerRequestsRequest());

            _logger.LogDebug($"End Get Aggregated Employer Requests Handler");
            return result;
        }
    }
}
