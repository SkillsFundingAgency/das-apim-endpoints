using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetLEPSDataWithUsers
{
    public class GetLEPSDataWithUsersQueryHandler : IRequestHandler<GetLEPSDataWithUsersQuery, GetLEPSDataWithUsersResult>
    {
        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public GetLEPSDataWithUsersQueryHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetLEPSDataWithUsersResult> Handle(GetLEPSDataWithUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.GetWithResponseCode<GetLEPSDataWithUsersResponse>(new GetLEPSDataWithUsersRequest());

            result.EnsureSuccessStatusCode();

            return new GetLEPSDataWithUsersResult
            {
                LEPSData = result.Body.LEPSData
            };
        }
    }
}