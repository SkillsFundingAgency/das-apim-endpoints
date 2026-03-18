using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetSettings
{
    public class GetSettingsQueryHandler : IRequestHandler<GetSettingsQuery, GetSettingsResult>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeshipTrainingApiClient;

        public GetSettingsQueryHandler(
            IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient)
        {
            _requestApprenticeshipTrainingApiClient = requestApprenticeTrainingApiClient;
        }

        public async Task<GetSettingsResult> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
        {
            var settings = await _requestApprenticeshipTrainingApiClient.
                Get<GetSettingsResponse>(new GetSettingsRequest());

            return new GetSettingsResult
            {
                ExpiryAfterMonths = settings.ExpiryAfterMonths,
                RemovedAfterExpiryMonths = settings.EmployerRemovedAfterExpiryMonths
            };
        }
    }
}
