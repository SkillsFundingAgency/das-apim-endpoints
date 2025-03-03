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

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds
{
    public class GetEmployerRequestsByIdsQueryHandler : IRequestHandler<GetEmployerRequestsByIdsQuery, GetEmployerRequestsByIdsResult>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeshipTrainingApiClient;
        private readonly ILogger<GetEmployerRequestsByIdsQueryHandler> _logger;

        public GetEmployerRequestsByIdsQueryHandler(
            IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient,
            ILogger<GetEmployerRequestsByIdsQueryHandler> logger)
        {
            _requestApprenticeshipTrainingApiClient = requestApprenticeTrainingApiClient;
            _logger = logger;
        }

        public async Task<GetEmployerRequestsByIdsResult> Handle(GetEmployerRequestsByIdsQuery request, CancellationToken cancellationToken)
        {
            var employerRequests = await _requestApprenticeshipTrainingApiClient.
                Get<List<GetEmployerRequestsByIdsResponse>>(new GetEmployerRequestsByIdsRequest(
                    request.EmployerRequestIds));

            return new GetEmployerRequestsByIdsResult
            {
                EmployerRequests = employerRequests.ToList()
            };
        }
    }
}
