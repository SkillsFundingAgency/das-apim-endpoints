using MediatR;
using SFA.DAS.EarlyConnect.Application.Commands.CreateLogData;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateMetricData
{
    public class CreateMetricDataCommandHandler : IRequestHandler<CreateMetricDataCommand, CreateMetricsDataCommandResult>
    {

        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public CreateMetricDataCommandHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CreateMetricsDataCommandResult> Handle(CreateMetricDataCommand request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.PostWithResponseCode<CreateMetricsDataCommandResult>(new CreateMetricDataRequest(request.metricsData), true);

            return new CreateMetricsDataCommandResult
            {
                StatusCode = response.StatusCode,
                ErrorMessage = response.ErrorContent
            };
        }
    }
}
