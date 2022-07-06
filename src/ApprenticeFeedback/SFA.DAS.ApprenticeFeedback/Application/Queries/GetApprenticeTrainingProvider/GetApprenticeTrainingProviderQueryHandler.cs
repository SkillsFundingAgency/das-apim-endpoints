using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprenticeTrainingProvider
{
    public class GetApprenticeTrainingProviderQueryHandler : IRequestHandler<GetApprenticeTrainingProviderQuery, GetApprenticeTrainingProviderResult>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly ILogger<GetApprenticeTrainingProviderQueryHandler> _logger;

        public GetApprenticeTrainingProviderQueryHandler(
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            ILogger<GetApprenticeTrainingProviderQueryHandler> logger)
        {
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _logger = logger;
        }

        public async Task<GetApprenticeTrainingProviderResult> Handle(GetApprenticeTrainingProviderQuery request, CancellationToken cancellationToken)
        {
            var result = await _apprenticeFeedbackApiClient.
               Get<GetApprenticeTrainingProviderResult>(new GetTrainingProviderForApprenticeRequest { ApprenticeId = request.ApprenticeId, Ukprn = request.Ukprn });

            _logger.LogDebug($"End GetApprenticeshipTrainingProviderQueryHandler for ApprenticeId:{request.ApprenticeId} and Ukprn:{request.Ukprn}");
            return result;
        }
    }
}
