using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests
{
    public class GetAggregatedEmployerRequestsQueryHandler : IRequestHandler<GetAggregatedEmployerRequestsQuery, GetAggregatedEmployerRequestsResult>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeshipTrainingApiClient;
        private readonly ILogger<GetAggregatedEmployerRequestsQueryHandler> _logger;

        public GetAggregatedEmployerRequestsQueryHandler(
            IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient,
            ILogger<GetAggregatedEmployerRequestsQueryHandler> logger)
        {
            _requestApprenticeshipTrainingApiClient = requestApprenticeTrainingApiClient;
            _logger = logger;
        }

        public async Task<GetAggregatedEmployerRequestsResult> Handle(GetAggregatedEmployerRequestsQuery request, CancellationToken cancellationToken)
        {
            var aggregatedEmployerRequests = await _requestApprenticeshipTrainingApiClient.
                Get<List<GetAggregatedEmployerRequestsResponse>>(new GetAggregatedEmployerRequestsRequest(request.Ukprn));

            return new GetAggregatedEmployerRequestsResult
            {
                AggregatedEmployerRequests = aggregatedEmployerRequests.ToList()
            };
        }
    }
}
