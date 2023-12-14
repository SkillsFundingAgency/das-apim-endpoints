using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateLogData
{
    public class CreateLogDataCommandHandler : IRequestHandler<CreateLogDataCommand, CreateLogDataCommandResult>
    {

        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;

        public CreateLogDataCommandHandler(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CreateLogDataCommandResult> Handle(CreateLogDataCommand request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.PostWithResponseCode<CreateLogDataResponse>(new CreateLogDataRequest(request.Log));

            response.EnsureSuccessStatusCode();

            return new CreateLogDataCommandResult
            {
                LogId = response.Body.LogId
            };
        }
    }
}
