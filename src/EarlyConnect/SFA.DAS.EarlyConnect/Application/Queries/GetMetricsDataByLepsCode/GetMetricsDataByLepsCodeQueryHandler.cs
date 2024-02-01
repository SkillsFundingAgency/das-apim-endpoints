using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetMetricsDataByLepsCode
{
    public class GetMetricsDataByLepsCodeQueryHandler : IRequestHandler<GetMetricsDataByLepsCodeQuery, GetMetricsDataByLepsCodeResult>
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public GetMetricsDataByLepsCodeQueryHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetMetricsDataByLepsCodeResult> Handle(GetMetricsDataByLepsCodeQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetWithResponseCode<GetMetricsDataByLepsCodeResponse>(new GetMetricsDataByLepsCodeRequest(request.LepsCode));

            result.EnsureSuccessStatusCode();

            return new GetMetricsDataByLepsCodeResult
            {
                ListOfMetricsData = result.Body.ListOfMetricsData
            };
        }
    }
}