using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation
{
    public class GetProviderResponseConfirmationQueryHandler : IRequestHandler<GetProviderResponseConfirmationQuery, GetProviderResponseConfirmationResult>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeshipTrainingApiClient;
        private readonly ILogger<GetProviderResponseConfirmationQueryHandler> _logger;

        public GetProviderResponseConfirmationQueryHandler(
            IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient,
            ILogger<GetProviderResponseConfirmationQueryHandler> logger)
        {
            _requestApprenticeshipTrainingApiClient = requestApprenticeTrainingApiClient;
            _logger = logger;
        }

        public async Task<GetProviderResponseConfirmationResult> Handle(GetProviderResponseConfirmationQuery request, CancellationToken cancellationToken)
        {
            var providerResponseConfirmation = await _requestApprenticeshipTrainingApiClient.
                Get<GetProviderResponseConfirmationResponse>(new GetProviderResponseConfirmationRequest(
                    request.ProviderResponseId));

            return new GetProviderResponseConfirmationResult
            {
                Ukprn = providerResponseConfirmation.Ukprn,
                Email = providerResponseConfirmation.Email,
                Phone = providerResponseConfirmation.Phone,
                Website = providerResponseConfirmation.Website,
                EmployerRequests = providerResponseConfirmation.EmployerRequests,
            };
        }
    }
}
