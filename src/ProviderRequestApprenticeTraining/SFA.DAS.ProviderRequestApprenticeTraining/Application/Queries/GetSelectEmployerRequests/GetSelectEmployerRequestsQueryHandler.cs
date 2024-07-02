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

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests
{
    public class GetSelectEmployerRequestsQueryHandler : IRequestHandler<GetSelectEmployerRequestsQuery, GetSelectEmployerRequestsResult>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeshipTrainingApiClient;
        private readonly ILogger<GetSelectEmployerRequestsQueryHandler> _logger;

        public GetSelectEmployerRequestsQueryHandler(
            IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient,
            ILogger<GetSelectEmployerRequestsQueryHandler> logger)
        {
            _requestApprenticeshipTrainingApiClient = requestApprenticeTrainingApiClient;
            _logger = logger;
        }

        public async Task<GetSelectEmployerRequestsResult> Handle(GetSelectEmployerRequestsQuery request, CancellationToken cancellationToken)
        {
            var selectEmployerRequests = await _requestApprenticeshipTrainingApiClient.
                Get<List<GetSelectEmployerRequestsResponse>>(new GetSelectEmployerRequestsRequest(
                    request.StandardReference,
                    request.Ukprn));

            return new GetSelectEmployerRequestsResult
            {
                SelectEmployerRequests = selectEmployerRequests.ToList()
            };
        }
    }
}
