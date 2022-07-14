using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprenticeTrainingProviders
{
    public class GetApprenticeTrainingProvidersQueryHandler : IRequestHandler<GetApprenticeTrainingProvidersQuery, GetApprenticeTrainingProvidersResult>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly ILogger<GetApprenticeTrainingProvidersQueryHandler> _logger;

        public GetApprenticeTrainingProvidersQueryHandler(
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            ILogger<GetApprenticeTrainingProvidersQueryHandler> logger)
        {
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _logger = logger;
        }

        public async Task<GetApprenticeTrainingProvidersResult> Handle(GetApprenticeTrainingProvidersQuery request, CancellationToken cancellationToken)
        {
            var result = await _apprenticeFeedbackApiClient.
               Get<GetApprenticeTrainingProvidersResult>(new GetAllTrainingProvidersForApprenticeRequest { ApprenticeId = request.ApprenticeId });

            _logger.LogDebug($"End GetApprenticeshipTrainingProvidersQueryHandler for ApprenticeId:{request.ApprenticeId}");
            return result;
        }
    }
}
