using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.MetricData
{
    public class CreateMetricDataCommandHandler : IRequestHandler<CreateMetricDataCommand, Unit>
    {

        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public CreateMetricDataCommandHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<Unit> Handle(CreateMetricDataCommand request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.PostWithResponseCode<InnerApi.Requests.MetricData>(new CreateMetricDataRequest(request.MetricDataList), false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
